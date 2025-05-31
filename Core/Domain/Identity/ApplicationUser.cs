using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}".Trim();

        public string? ProfileImageUrl { get; private set; }

        public DateTime? DateOfBirth { get; private set; }

        public Gender? Gender { get; private set; }

        public decimal? Height { get; private set; } // in cm

        public decimal? Weight { get; private set; } // in kg

        public decimal? BMI => Height.HasValue && Weight.HasValue && Height > 0
            ? Math.Round(Weight.Value / (Height.Value / 100 * Height.Value / 100), 2)
            : null;

        public string? Address { get; private set; }

        public string? EmergencyContact { get; private set; }

        public string? EmergencyContactPhone { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? LastLoginAt { get; private set; }

        public bool IsProfileComplete =>
            !string.IsNullOrEmpty(FirstName) &&
            !string.IsNullOrEmpty(LastName) &&
            DateOfBirth.HasValue &&
            Gender.HasValue;

        public bool IsActive { get; private set; } = true;

        public DateTime? DeactivatedAt { get; private set; }

        public string? DeactivationReason { get; private set; }

        // Subscription related properties
        public UserSubscription? ActiveSubscription =>
            Subscriptions?.FirstOrDefault(s => s.IsActive && !s.IsCancelled);

        public bool HasActiveSubscription => ActiveSubscription != null;

        public SubscriptionType? CurrentSubscriptionType => ActiveSubscription?.Type;

        public Trainer? AssignedTrainer => ActiveSubscription?.AssignedTrainer;

        // Age calculation
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;

                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;

                if (DateOfBirth.Value.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        // Navigational Properties
        public ICollection<TrainerRating> TrainerRatings { get; private set; } = new List<TrainerRating>();
        public ICollection<UserSubscription> Subscriptions { get; private set; } = new List<UserSubscription>();
        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
        public ICollection<DietPlan> DietPlans { get; private set; } = new List<DietPlan>();
        public ICollection<WorkoutPlan> WorkoutPlans { get; private set; } = new List<WorkoutPlan>();

        // Private constructor for EF Core
        private ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public ApplicationUser(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateRequiredFields(firstName, lastName, email, phoneNumber);

            Id = Guid.NewGuid();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber.Trim();
            UserName = Email;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateProfile(
            string firstName,
            string lastName,
            string? profileImageUrl = null,
            DateTime? dateOfBirth = null,
            Gender? gender = null,
            decimal? height = null,
            decimal? weight = null,
            string? address = null)
        {
            ValidateNames(firstName, lastName);
            ValidatePhysicalMeasurements(height, weight);
            ValidateDateOfBirth(dateOfBirth);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            ProfileImageUrl = profileImageUrl;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Height = height;
            Weight = weight;
            Address = address?.Trim();
        }

        public void UpdateContactInfo(string email, string phoneNumber)
        {
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);

            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber.Trim();
            UserName = Email;
        }

        public void SetEmergencyContact(string contactName, string contactPhone)
        {
            if (string.IsNullOrWhiteSpace(contactName))
                throw new ArgumentException("Təcili əlaqə adı boş ola bilməz.");

            if (string.IsNullOrWhiteSpace(contactPhone))
                throw new ArgumentException("Təcili əlaqə telefonu boş ola bilməz.");

            EmergencyContact = contactName.Trim();
            EmergencyContactPhone = contactPhone.Trim();
        }

        public void UpdateProfileImage(string? imageUrl)
        {
            ProfileImageUrl = imageUrl;
        }

        public void UpdateLastLoginTime()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Deactivate(string? reason = null)
        {
            if (!IsActive)
                throw new InvalidOperationException("İstifadəçi artıq deaktivdir.");

            IsActive = false;
            DeactivatedAt = DateTime.UtcNow;
            DeactivationReason = reason;
        }

        public void Reactivate()
        {
            if (IsActive)
                throw new InvalidOperationException("İstifadəçi artıq aktivdir.");

            IsActive = true;
            DeactivatedAt = null;
            DeactivationReason = null;
        }

        public bool CanSubscribeTo(SubscriptionType subscriptionType)
        {
            if (!IsActive)
                return false;

            var activeSubscription = ActiveSubscription;

            // If no active subscription, can subscribe to any type
            if (activeSubscription == null)
                return true;

            // Can't downgrade to trial
            if (subscriptionType == SubscriptionType.Trial)
                return false;

            // Can upgrade or change to different type
            return true;
        }

        // Validation methods
        private static void ValidateRequiredFields(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateNames(firstName, lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);
        }

        private static void ValidateNames(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ad boş ola bilməz.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Soyad boş ola bilməz.");

            if (firstName.Trim().Length < 2)
                throw new ArgumentException("Ad ən azı 2 simvol olmalıdır.");

            if (lastName.Trim().Length < 2)
                throw new ArgumentException("Soyad ən azı 2 simvol olmalıdır.");
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş ola bilməz.");

            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Email formatı düzgün deyil.");
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Telefon nömrəsi boş ola bilməz.");

            if (phoneNumber.Trim().Length < 10)
                throw new ArgumentException("Telefon nömrəsi ən azı 10 rəqəm olmalıdır.");
        }

        private static void ValidatePhysicalMeasurements(decimal? height, decimal? weight)
        {
            if (height.HasValue && (height <= 0 || height > 300))
                throw new ArgumentException("Boy 0-300 sm arasında olmalıdır.");

            if (weight.HasValue && (weight <= 0 || weight > 500))
                throw new ArgumentException("Çəki 0-500 kq arasında olmalıdır.");
        }

        private static void ValidateDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue)
            {
                if (dateOfBirth.Value > DateTime.Today)
                    throw new ArgumentException("Doğum tarixi bugünkü tarixdən sonra ola bilməz.");

                if (dateOfBirth.Value < DateTime.Today.AddYears(-120))
                    throw new ArgumentException("Doğum tarixi 120 ildən çox əvvəl ola bilməz.");
            }
        }
    }
}

namespace Domain.Enums
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3,
        PreferNotToSay = 4
    }
}

namespace Domain.Extensions
{
    public static class GenderExtensions
    {
        public static string GetDisplayName(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Kişi",
                Gender.Female => "Qadın",
                Gender.Other => "Digər",
                Gender.PreferNotToSay => "Deməyi üstün tutmuram",
                _ => gender.ToString()
            };
        }
    }
}