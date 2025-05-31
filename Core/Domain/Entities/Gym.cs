using Domain.Common;
using Domain.Common.Helpers;
using Domain.Enums;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class Gym : BaseAuditableEntity
    {
        private readonly List<GymFacility> _facilities = new();
        private readonly List<Trainer> _trainers = new();

        private Gym() { }

        private Gym(GymInfo info, GymLocation location, GymSettings settings)
        {
            Name = info.Name;
            Description = info.Description;
            Address = info.Address;
            City = info.City;
            PhoneNumber = info.PhoneNumber;
            Email = info.Email;
            MonthlyPrice = info.MonthlyPrice;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            IsVipSupported = settings.IsVipSupported;
            IsCorporatePartner = settings.IsCorporatePartner;
        }

        // Properties
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string Address { get; private set; }
        public virtual City City { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public bool IsVipSupported { get; private set; }
        public bool IsCorporatePartner { get; private set; }
        public decimal MonthlyPrice { get; private set; }

        // Computed Properties
        public SubcriptionType SubscriptionCategory =>
            SubcriptionTypeHelper.DetermineSubcriptionType(MonthlyPrice);

        public IReadOnlyCollection<GymFacility> Facilities => _facilities.AsReadOnly();
        public IReadOnlyCollection<Trainer> Trainers => _trainers.AsReadOnly();

        // Factory Method
        public static Gym Create(GymInfo info, GymLocation location, GymSettings settings)
        {
            ValidateGymInfo(info);
            ValidateLocation(location);

            return new Gym(info, location, settings);
        }

        // Update Methods
        public void UpdateInfo(GymInfo info)
        {
            ValidateGymInfo(info);

            Name = info.Name;
            Description = info.Description;
            Address = info.Address;
            City = info.City;
            PhoneNumber = info.PhoneNumber;
            Email = info.Email;
            MonthlyPrice = info.MonthlyPrice;
        }

        public void UpdateLocation(GymLocation location)
        {
            ValidateLocation(location);

            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }

        public void UpdateSettings(GymSettings settings)
        {
            IsVipSupported = settings.IsVipSupported;
            IsCorporatePartner = settings.IsCorporatePartner;
        }

        public void UpdateMonthlyPrice(decimal monthlyPrice)
        {
            if (monthlyPrice < 0)
                throw new ArgumentException("Monthly price cannot be negative");

            MonthlyPrice = monthlyPrice;
        }

        // Facility Management
        public void AddFacility(GymFacility facility)
        {
            if (facility == null)
                throw new ArgumentNullException(nameof(facility));

            if (facility.GymId != Id)
                throw new InvalidOperationException("Facility must belong to this gym");

            if (_facilities.Any(f => f.Id == facility.Id))
                throw new InvalidOperationException("Facility already exists in this gym");

            _facilities.Add(facility);
        }

        public void RemoveFacility(Guid facilityId)
        {
            var facility = _facilities.FirstOrDefault(f => f.Id == facilityId);
            if (facility != null)
                _facilities.Remove(facility);
        }

        public bool HasFacility(Guid facilityId)
        {
            return _facilities.Any(f => f.Id == facilityId);
        }

        // Trainer Management
        public void AddTrainer(Trainer trainer)
        {
            if (trainer == null)
                throw new ArgumentNullException(nameof(trainer));

            if (_trainers.Any(t => t.Id == trainer.Id))
                throw new InvalidOperationException("Trainer already exists in this gym");

            _trainers.Add(trainer);
        }

        public void RemoveTrainer(Guid trainerId)
        {
            var trainer = _trainers.FirstOrDefault(t => t.Id == trainerId);
            if (trainer != null)
                _trainers.Remove(trainer);
        }

        public bool HasTrainer(Guid trainerId)
        {
            return _trainers.Any(t => t.Id == trainerId);
        }

        // Utility Methods
        public double DistanceFrom(double latitude, double longitude)
        {
            return CalculateDistance(Latitude, Longitude, latitude, longitude);
        }

        public bool IsNearby(double latitude, double longitude, double radiusKm = 5.0)
        {
            return DistanceFrom(latitude, longitude) <= radiusKm;
        }

        public bool IsAffordableFor(decimal budget)
        {
            return MonthlyPrice <= budget;
        }

        // Validation Methods
        private static void ValidateGymInfo(GymInfo info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (string.IsNullOrWhiteSpace(info.Name))
                throw new ArgumentException("Gym name is required");

            if (info.Name.Length > 100)
                throw new ArgumentException("Gym name cannot exceed 100 characters");

            if (string.IsNullOrWhiteSpace(info.Address))
                throw new ArgumentException("Address is required");

            if (info.Address.Length > 500)
                throw new ArgumentException("Address cannot exceed 500 characters");

            if (string.IsNullOrWhiteSpace(info.PhoneNumber))
                throw new ArgumentException("Phone number is required");

            if (!IsValidPhoneNumber(info.PhoneNumber))
                throw new ArgumentException("Invalid phone number format");

            if (!string.IsNullOrEmpty(info.Email) && !IsValidEmail(info.Email))
                throw new ArgumentException("Invalid email format");

            if (info.MonthlyPrice < 0)
                throw new ArgumentException("Monthly price cannot be negative");

            if (info.MonthlyPrice > 10000)
                throw new ArgumentException("Monthly price seems unrealistic (>10000)");

            if (info.City == null)
                throw new ArgumentException("City is required");
        }

        private static void ValidateLocation(GymLocation location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (location.Latitude < -90 || location.Latitude > 90)
                throw new ArgumentException("Latitude must be between -90 and 90");

            if (location.Longitude < -180 || location.Longitude > 180)
                throw new ArgumentException("Longitude must be between -180 and 180");
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Simple phone validation - can be enhanced based on requirements
            return Regex.IsMatch(phoneNumber, @"^[\+]?[0-9\s\-\(\)]{10,15}$");
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double degrees) => degrees * (Math.PI / 180);

        public override string ToString() => $"{Name} - {City?.Name} ({MonthlyPrice:C}/month)";

        public override bool Equals(object obj) =>
            obj is Gym other && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }

    // Helper Classes
    public class GymInfo
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public string Address { get; init; }
        public City City { get; init; }
        public string PhoneNumber { get; init; }
        public string? Email { get; init; }
        public decimal MonthlyPrice { get; init; }
    }

    public class GymLocation
    {
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }

    public class GymSettings
    {
        public bool IsVipSupported { get; init; }
        public bool IsCorporatePartner { get; init; }
    }
}