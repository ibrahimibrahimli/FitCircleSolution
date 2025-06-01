using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class Payment : BaseAuditableEntity
    {
        public Guid UserSubscriptionId { get; private set; }
        public UserSubscription UserSubscription { get; private set; } = null!;

        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; } = null!;

        public decimal Amount { get; private set; }

        public string Currency { get; private set; } = "AZN";

        public PaymentStatus Status { get; private set; }

        public PaymentMethod Method { get; private set; }

        public DateTime PaymentDate { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public string? TransactionId { get; private set; }

        public string? ExternalReference { get; private set; }

        public string? Description { get; private set; }

        public string? FailureReason { get; private set; }

        public decimal? RefundedAmount { get; private set; }

        public DateTime? RefundedAt { get; private set; }

        public RefundReason? RefundReason { get; private set; }

        public string? RefundNotes { get; private set; }

        public string? ProcessorResponse { get; private set; }

        public bool IsCompleted => Status == PaymentStatus.Completed;

        public bool IsPending => Status == PaymentStatus.Pending;

        public bool IsFailed => Status == PaymentStatus.Failed;

        public bool IsRefunded => Status == PaymentStatus.Refunded || Status == PaymentStatus.PartiallyRefunded;

        public decimal RefundableAmount => IsCompleted ? Amount - (RefundedAmount ?? 0) : 0;

        public bool CanBeRefunded => IsCompleted && RefundableAmount > 0;

        // Private constructor for EF Core
        private Payment() { }

        public static Payment Create(
            Guid userSubscriptionId,
            Guid userId,
            decimal amount,
            PaymentMethod method,
            string currency = "AZN",
            string? description = null,
            string? externalReference = null)
        {
            ValidateAmount(amount);

            return new Payment
            {
                Id = Guid.NewGuid(),
                UserSubscriptionId = userSubscriptionId,
                UserId = userId,
                Amount = amount,
                Currency = currency,
                Method = method,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow,
                Description = description,
                ExternalReference = externalReference
            };
        }

        public void MarkAsCompleted(string? transactionId = null, string? processorResponse = null)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Yalnız gözləyən ödənişlər tamamlana bilər.");

            Status = PaymentStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            TransactionId = transactionId;
            ProcessorResponse = processorResponse;
            FailureReason = null;
        }

        public void MarkAsFailed(string failureReason, string? processorResponse = null)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Yalnız gözləyən ödənişlər uğursuz ola bilər.");

            Status = PaymentStatus.Failed;
            FailureReason = failureReason;
            ProcessorResponse = processorResponse;
        }

        public void Cancel()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Yalnız gözləyən ödənişlər ləğv edilə bilər.");

            Status = PaymentStatus.Cancelled;
        }

        public void ProcessFullRefund(RefundReason reason, string? notes = null)
        {
            if (!CanBeRefunded)
                throw new InvalidOperationException("Bu ödəniş geri qaytarıla bilməz.");

            var refundAmount = RefundableAmount;
            ProcessRefund(refundAmount, reason, notes);
        }

        public void ProcessPartialRefund(decimal refundAmount, RefundReason reason, string? notes = null)
        {
            if (!CanBeRefunded)
                throw new InvalidOperationException("Bu ödəniş geri qaytarıla bilməz.");

            if (refundAmount <= 0 || refundAmount > RefundableAmount)
                throw new ArgumentException("Geri qaytarılacaq məbləğ düzgün deyil.");

            ProcessRefund(refundAmount, reason, notes);
        }

        private void ProcessRefund(decimal refundAmount, RefundReason reason, string? notes)
        {
            RefundedAmount = (RefundedAmount ?? 0) + refundAmount;
            RefundedAt = DateTime.UtcNow;
            RefundReason = reason;
            RefundNotes = notes;

            Status = RefundedAmount >= Amount
                ? PaymentStatus.Refunded
                : PaymentStatus.PartiallyRefunded;
        }

        public void UpdateTransactionId(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Tranzaksiya ID-si boş ola bilməz.");

            TransactionId = transactionId;
        }

        public void UpdateExternalReference(string externalReference)
        {
            ExternalReference = externalReference;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Ödəniş məbləği sıfırdan böyük olmalıdır.");
        }
    }
}

namespace Domain.Extensions
{
    public static class SubscriptionTypeExtensions
    {
        public static string GetDisplayName(this SubscriptionType type)
        {
            return type switch
            {
                SubscriptionType.Basic => "Əsas",
                SubscriptionType.Premium => "Premium",
                SubscriptionType.Vip => "VIP",
                SubscriptionType.Corporate => "Korporativ",
                _ => type.ToString()
            };
        }

        public static int GetDurationInDays(this SubscriptionType type)
        {
            return type switch
            {
                SubscriptionType.Basic => 30,
                SubscriptionType.Premium => 30,
                SubscriptionType.Vip => 30,
                SubscriptionType.Corporate => 365,
                _ => 30
            };
        }

        public static decimal GetBasePrice(this SubscriptionType type)
        {
            return type switch
            {
                SubscriptionType.Basic => 50m,
                SubscriptionType.Premium => 100m,
                SubscriptionType.Vip => 200m,
                SubscriptionType.Corporate => 1000m,
                _ => 0m
            };
        }

        public static bool RequiresTrainer(this SubscriptionType type)
        {
            return type switch
            {
                SubscriptionType.Premium => true,
                SubscriptionType.Vip => true,
                SubscriptionType.Corporate => true,
                _ => false
            };
        }
    }

    public static class PaymentStatusExtensions
    {
        public static string GetDisplayName(this PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Gözləyir",
                PaymentStatus.Completed => "Tamamlandı",
                PaymentStatus.Failed => "Uğursuz",
                PaymentStatus.Cancelled => "Ləğv edildi",
                PaymentStatus.Refunded => "Geri qaytarıldı",
                PaymentStatus.PartiallyRefunded => "Qismən geri qaytarıldı",
                _ => status.ToString()
            };
        }
    }

    public static class PaymentMethodExtensions
    {
        public static string GetDisplayName(this PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.CreditCard => "Bank kartı",
                PaymentMethod.BankTransfer => "Bank köçürməsi",
                PaymentMethod.DebitCard => "Debet Cart",
                PaymentMethod.GooglePay => "Google Pay",
                PaymentMethod.ApplePay => "Apple Pay",
                _ => method.ToString()
            };
        }
    }
}