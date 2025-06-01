using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class GymFacility : BaseAuditableEntity
{
    private GymFacility() { } // ORM üçün

    private GymFacility(string name, string description, GymFacilityType facilityType, Guid gymId, int? maxCapacity = null)
    {
        Name = name;
        Description = description;
        FacilityType = facilityType;
        GymId = gymId;
        MaxCapacity = maxCapacity ?? facilityType.DefaultMaxCapacity;
        HourlyRate = facilityType.GetBaseHourlyRate();
        IsAvailable = true; // Default available
    }

    // Core Properties
    public string Name { get; private set; }
    public string Description { get; private set; }
    public GymFacilityType FacilityType { get; private set; } = GymFacilityType.Cardio;
    public Guid GymId { get; private set; }
    public virtual Gym Gym { get; private set; }

    // Additional Properties from GymFacilityType
    public int MaxCapacity { get; private set; }
    public decimal HourlyRate { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTime? MaintenanceScheduled { get; private set; }
    public int CurrentOccupancy { get; private set; } = 0;

    // Factory Method
    public static GymFacility Create(string name, string description, GymFacilityType facilityType, Guid gymId, int? maxCapacity = null)
    {
        ValidateInput(name, description, facilityType, gymId, maxCapacity);
        return new GymFacility(name, description, facilityType, gymId, maxCapacity);
    }

    // Update Methods
    public void Update(string name, string description, GymFacilityType facilityType)
    {
        ValidateBasicInput(name, description, facilityType);

        Name = name;
        Description = description;

        // If facility type changes, update related properties
        if (FacilityType != facilityType)
        {
            FacilityType = facilityType;
            MaxCapacity = facilityType.DefaultMaxCapacity;
            HourlyRate = facilityType.GetBaseHourlyRate();
        }
    }

    public void UpdateCapacity(int maxCapacity)
    {
        if (maxCapacity <= 0)
            throw new ArgumentException("Capacity must be positive");

        if (maxCapacity < CurrentOccupancy)
            throw new InvalidOperationException("Cannot set capacity lower than current occupancy");

        MaxCapacity = maxCapacity;
    }

    public void UpdateHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate < 0)
            throw new ArgumentException("Hourly rate cannot be negative");

        HourlyRate = hourlyRate;
    }

    public void SetAvailability(bool isAvailable, string? reason = null)
    {
        IsAvailable = isAvailable;

        // Clear occupancy when facility becomes unavailable
        if (!isAvailable)
        {
            CurrentOccupancy = 0;
        }

        // Optionally log reason for unavailability
        if (!isAvailable && !string.IsNullOrEmpty(reason))
        {
            // Could add a reason property or logging here
        }
    }

    public void ScheduleMaintenance(DateTime maintenanceDate)
    {
        if (maintenanceDate <= DateTime.Now)
            throw new ArgumentException("Maintenance date must be in the future");

        MaintenanceScheduled = maintenanceDate;
        IsAvailable = false; // Temporarily unavailable
        CurrentOccupancy = 0; // Clear occupancy during maintenance
    }

    public void CompleteMaintenance()
    {
        MaintenanceScheduled = null;
        IsAvailable = true;
    }

    // Occupancy Management
    public void CheckIn(int count = 1)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Facility is not available");

        if (CurrentOccupancy + count > MaxCapacity)
            throw new InvalidOperationException("Facility capacity would be exceeded");

        CurrentOccupancy += count;
    }

    public void CheckOut(int count = 1)
    {
        if (CurrentOccupancy < count)
            throw new InvalidOperationException("Cannot check out more people than are currently in the facility");

        CurrentOccupancy -= count;
    }

    // Business Logic Methods using GymFacilityType
    public bool IsCompatibleWith(GymFacilityType otherType)
    {
        return FacilityType.IsCompatibleWith(otherType);
    }

    public bool CanAccommodate(int requestedCapacity)
    {
        if (!IsAvailable)
            return false;

        return CurrentOccupancy + requestedCapacity <= MaxCapacity;
    }

    public bool RequiresMaintenance()
    {
        return MaintenanceScheduled.HasValue && MaintenanceScheduled.Value <= DateTime.Now.AddDays(7);
    }

    public bool RequiresSpecialCertification()
    {
        return FacilityType.RequiresSpecialCertification();
    }

    public TimeSpan GetRecommendedSessionDuration()
    {
        return FacilityType.GetRecommendedSessionDuration();
    }

    public bool RequiresEquipment()
    {
        return FacilityType.RequiresEquipment;
    }

    public string GetStatusDescription()
    {
        if (!IsAvailable)
        {
            if (MaintenanceScheduled.HasValue)
                return $"Under maintenance until {MaintenanceScheduled.Value:dd/MM/yyyy}";
            return "Temporarily unavailable";
        }

        if (RequiresMaintenance())
            return $"Maintenance scheduled for {MaintenanceScheduled.Value:dd/MM/yyyy}";

        if (CurrentOccupancy >= MaxCapacity)
            return "At full capacity";

        return $"Available ({CurrentOccupancy}/{MaxCapacity})";
    }

    public decimal CalculateSessionCost(TimeSpan duration)
    {
        var hours = (decimal)duration.TotalHours;
        return Math.Round(HourlyRate * hours, 2);
    }

    // Validation Methods
    private static void ValidateInput(string name, string description, GymFacilityType facilityType, Guid gymId, int? maxCapacity)
    {
        ValidateBasicInput(name, description, facilityType);

        if (gymId == Guid.Empty)
            throw new ArgumentException("Gym ID is required");

        if (maxCapacity.HasValue && maxCapacity.Value <= 0)
            throw new ArgumentException("Capacity must be positive");
    }

    private static void ValidateBasicInput(string name, string description, GymFacilityType facilityType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Facility name is required");

        if (name.Length > 100)
            throw new ArgumentException("Facility name cannot exceed 100 characters");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");

        if (description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters");

        if (facilityType == null)
            throw new ArgumentNullException(nameof(facilityType), "Facility type is required");
    }

    public override string ToString() => $"{FacilityType.Icon} {Name} - {GetStatusDescription()}";

    public override bool Equals(object obj) =>
        obj is GymFacility other && Id == other.Id;

    public override int GetHashCode() => Id.GetHashCode();
}