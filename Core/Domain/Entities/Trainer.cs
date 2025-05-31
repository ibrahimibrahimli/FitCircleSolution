using System.Text.RegularExpressions;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Trainer : BaseAuditableEntity
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new(@"^\+?[\d\s\-\(\)]{7,15}$", RegexOptions.Compiled);
        private static readonly Regex InstagramRegex = new(@"^[a-zA-Z0-9._]{1,30}$", RegexOptions.Compiled);

        private Trainer() { }

        private Trainer(
            string firstname,
            string lastname,
            string email,
            string phoneNumber,
            string bio,
            Guid gymId,
            string? profileImageUrl,
            string? coverImageUrl,
            string instagramHandle)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PhoneNumber = phoneNumber;
            Bio = bio;
            GymId = gymId;
            ProfileImageUrl = profileImageUrl;
            CoverImageUrl = coverImageUrl;
            InstagramHandle = instagramHandle;
            IsActive = true;
            YearsOfExperience = 0;
        }

        // Basic Properties
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Bio { get; private set; }
        public Guid GymId { get; private set; }
        public Gym Gym { get; private set; }

        // Media Properties
        public string? ProfileImageUrl { get; private set; }
        public string? CoverImageUrl { get; private set; }
        public string InstagramHandle { get; private set; }

        // Professional Properties
        public bool IsActive { get; private set; }
        public int YearsOfExperience { get; private set; }
        public decimal HourlyRate { get; private set; }
        public DateTime? HiredDate { get; private set; }

        // Collections
        private readonly List<TrainerRating> _ratings = new();
        private readonly List<GymFacilityType> _specializations = new();
        private readonly List<string> _certifications = new();

        public IReadOnlyCollection<TrainerRating> Ratings => _ratings.AsReadOnly();
        public IReadOnlyCollection<GymFacilityType> Specializations => _specializations.AsReadOnly();
        public IReadOnlyCollection<string> Certifications => _certifications.AsReadOnly();

        // Computed Properties
        public string FullName => $"{Firstname} {Lastname}";
        public double AverageRating => _ratings.Any() ? _ratings.Average(r => r.Rating) : 0.0;
        public int TotalRatings => _ratings.Count;
        public bool HasSpecialCertifications => _specializations.Any(s => s.RequiresSpecialCertification());

        // Factory Method
        public static Trainer Create(
            string firstname,
            string lastname,
            string email,
            string phoneNumber,
            string bio,
            Guid gymId,
            string? profileImageUrl = null,
            string? coverImageUrl = null,
            string instagramHandle = "")
        {
            ValidateInput(firstname, lastname, email, phoneNumber, bio, gymId, instagramHandle);
            return new Trainer(firstname, lastname, email, phoneNumber, bio, gymId, profileImageUrl, coverImageUrl, instagramHandle);
        }

        // Update Methods
        public void UpdateProfile(
            string firstname,
            string lastname,
            string phoneNumber,
            string bio,
            string? profileImageUrl = null,
            string? coverImageUrl = null,
            string instagramHandle = "")
        {
            ValidateBasicInput(firstname, lastname, phoneNumber, bio, instagramHandle);

            Firstname = firstname;
            Lastname = lastname;
            PhoneNumber = phoneNumber;
            Bio = bio;
            ProfileImageUrl = profileImageUrl;
            CoverImageUrl = coverImageUrl;
            InstagramHandle = instagramHandle;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format");

            Email = email.ToLowerInvariant();
        }

        public void UpdateProfessionalInfo(int yearsOfExperience, decimal hourlyRate, DateTime? hiredDate = null)
        {
            if (yearsOfExperience < 0)
                throw new ArgumentException("Years of experience cannot be negative");

            if (hourlyRate < 0)
                throw new ArgumentException("Hourly rate cannot be negative");

            YearsOfExperience = yearsOfExperience;
            HourlyRate = hourlyRate;
            HiredDate = hiredDate ?? HiredDate ?? DateTime.UtcNow;
        }

        // Status Management
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        // Rating Management
        public void AddRating(TrainerRating rating)
        {
            if (rating == null)
                throw new ArgumentNullException(nameof(rating));

            if (rating.TrainerId != Id)
                throw new ArgumentException("Rating does not belong to this trainer");

            _ratings.Add(rating);
        }

        public void RemoveRating(Guid ratingId)
        {
            var rating = _ratings.FirstOrDefault(r => r.Id == ratingId);
            if (rating != null)
            {
                _ratings.Remove(rating);
            }
        }

        // Specialization Management
        public void AddSpecialization(GymFacilityType facilityType)
        {
            if (facilityType == null)
                throw new ArgumentNullException(nameof(facilityType));

            if (_specializations.Contains(facilityType))
                throw new InvalidOperationException("Trainer already has this specialization");

            _specializations.Add(facilityType);
        }

        public void RemoveSpecialization(GymFacilityType facilityType)
        {
            _specializations.Remove(facilityType);
        }

        public bool HasSpecialization(GymFacilityType facilityType)
        {
            return _specializations.Contains(facilityType);
        }

        // Certification Management
        public void AddCertification(string certification)
        {
            if (string.IsNullOrWhiteSpace(certification))
                throw new ArgumentException("Certification name is required");

            var trimmedCertification = certification.Trim();
            if (_certifications.Contains(trimmedCertification, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("Trainer already has this certification");

            _certifications.Add(trimmedCertification);
        }

        public void RemoveCertification(string certification)
        {
            if (string.IsNullOrWhiteSpace(certification))
                return;

            var existing = _certifications.FirstOrDefault(c =>
                c.Equals(certification.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                _certifications.Remove(existing);
            }
        }

        // Business Logic
        public bool CanTrainIn(GymFacilityType facilityType)
        {
            if (facilityType == null || !IsActive)
                return false;

            // If facility requires special certification, trainer must have specialization in it
            if (facilityType.RequiresSpecialCertification())
            {
                return HasSpecialization(facilityType);
            }

            return true;
        }

        public decimal CalculateSessionCost(TimeSpan duration)
        {
            if (HourlyRate <= 0)
                return 50m; // Default rate

            var hours = (decimal)duration.TotalHours;
            return Math.Round(HourlyRate * hours, 2);
        }

        public decimal CalculateSessionCost(TimeSpan duration, GymFacilityType facilityType)
        {
            var rate = HourlyRate > 0 ? HourlyRate : facilityType?.GetBaseHourlyRate() ?? 50m;
            var hours = (decimal)duration.TotalHours;
            return Math.Round(rate * hours, 2);
        }

        public bool IsExperiencedIn(GymFacilityType facilityType)
        {
            return HasSpecialization(facilityType) && YearsOfExperience >= 2;
        }

        // Validation Methods
        private static void ValidateInput(string firstname, string lastname, string email, string phoneNumber, string bio, Guid gymId, string instagramHandle)
        {
            ValidateBasicInput(firstname, lastname, phoneNumber, bio, instagramHandle);

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format");

            if (gymId == Guid.Empty)
                throw new ArgumentException("Gym ID is required");
        }

        private static void ValidateBasicInput(string firstname, string lastname, string phoneNumber, string bio, string instagramHandle)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ArgumentException("First name is required");

            if (firstname.Length > 50)
                throw new ArgumentException("First name cannot exceed 50 characters");

            if (string.IsNullOrWhiteSpace(lastname))
                throw new ArgumentException("Last name is required");

            if (lastname.Length > 50)
                throw new ArgumentException("Last name cannot exceed 50 characters");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required");

            if (!PhoneRegex.IsMatch(phoneNumber))
                throw new ArgumentException("Invalid phone number format");

            if (string.IsNullOrWhiteSpace(bio))
                throw new ArgumentException("Bio is required");

            if (bio.Length > 1000)
                throw new ArgumentException("Bio cannot exceed 1000 characters");

            if (!string.IsNullOrEmpty(instagramHandle) && !InstagramRegex.IsMatch(instagramHandle))
                throw new ArgumentException("Invalid Instagram handle format");
        }

        public override string ToString() => $"{FullName} - {(IsActive ? "Active" : "Inactive")} Trainer";

        public override bool Equals(object obj) =>
            obj is Trainer other && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }