using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class UserSubscription : BaseAuditableEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; } = null!;

        public SubscriptionType Type { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate && !IsCancelled;

        public bool IsCancelled { get; private set; }

        public DateTime? CancelledAt { get; private set; }

        public string? CancellationReason { get; private set; }

        public Guid? AssignedTrainerId { get; private set; }

        public Trainer? AssignedTrainer { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; } = "Tl";

        public bool AutoRenewal { get; private set; }

        public int RemainingDays => IsActive && !IsCancelled
            ? Math.Max(0, (EndDate.Date - DateTime.UtcNow.Date).Days)
            : 0;

        public bool IsExpiringSoon => IsActive && RemainingDays <= 7;

        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

        // Private constructor for EF Core
        private UserSubscription() { }

        public static UserSubscription Create(
            Guid userId,
            SubscriptionType type,
            DateTime startDate,
            DateTime endDate,
            decimal amount,
            bool autoRenewal = false,
            Guid? trainerId = null,
            string currency = "Tl")
        {
            ValidateSubscriptionPeriod(startDate, endDate);
            ValidateAmount(amount);

            return new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount,
                Currency = currency,
                AutoRenewal = autoRenewal,
                AssignedTrainerId = trainerId,
                IsCancelled = false
            };
        }

        public void Extend(DateTime newEndDate)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Ləğv edilmiş abunəlik uzadıla bilməz.");

            if (newEndDate <= EndDate)
                throw new InvalidOperationException("Yeni tarix mövcud tarixdən sonra olmalıdır.");

            EndDate = newEndDate;
        }

        public void AssignTrainer(Guid trainerId)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Ləğv edilmiş abunəliyə məşqçi təyin edilə bilməz.");

            AssignedTrainerId = trainerId;
        }

        public void RemoveTrainer()
        {
            AssignedTrainerId = null;
        }

        public void Cancel(string? reason = null)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Abunəlik artıq ləğv edilib.");

            IsCancelled = true;
            CancelledAt = DateTime.UtcNow;
            CancellationReason = reason;
        }

        public void Reactivate()
        {
            if (!IsCancelled)
                throw new InvalidOperationException("Abunəlik artıq aktivdir.");

            if (DateTime.UtcNow > EndDate)
                throw new InvalidOperationException("Müddəti bitmiş abunəlik yenidən aktivləşdirilə bilməz.");

            IsCancelled = false;
            CancelledAt = null;
            CancellationReason = null;
        }

        public void UpdateAmount(decimal newAmount)
        {
            ValidateAmount(newAmount);
            Amount = newAmount;
        }

        public void EnableAutoRenewal()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Ləğv edilmiş abunəlik üçün avtomatik yenilənmə aktivləşdirilə bilməz.");

            AutoRenewal = true;
        }

        public void DisableAutoRenewal()
        {
            AutoRenewal = false;
        }

        public UserSubscription Renew(DateTime newStartDate, DateTime newEndDate, decimal? newAmount = null)
        {
            if (!IsActive || IsCancelled)
                throw new InvalidOperationException("Yalnız aktiv abunəlik yenilənə bilər.");

            if (newStartDate < EndDate.Date)
                throw new InvalidOperationException("Yeni başlama tarixi mövcud bitmə tarixindən əvvəl ola bilməz.");

            return Create(
                UserId,
                Type,
                newStartDate,
                newEndDate,
                newAmount ?? Amount,
                AutoRenewal,
                AssignedTrainerId,
                Currency
            );
        }

        private static void ValidateSubscriptionPeriod(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Başlama tarixi bitmə tarixindən əvvəl olmalıdır.");

            if (endDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Bitmə tarixi bugünkü tarixdən sonra olmalıdır.");
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Məbləğ sıfırdan böyük olmalıdır.");
        }
    }
}