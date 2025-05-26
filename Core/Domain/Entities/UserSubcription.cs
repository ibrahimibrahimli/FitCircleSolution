using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class UserSubcription : BaseAuditableEntity
    {
        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; }
        public SubcriptionType Type { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

        public Guid? AssignedTrainerId { get; private set; }
        public Trainer AssignedTrainer { get; private set; }
        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();


        private UserSubcription(){}


        public static UserSubcription Create(Guid userId, SubcriptionType type, DateTime start, DateTime end, Guid? trainerId = null)
        {
            return new UserSubcription
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = type,
                StartDate = start,
                EndDate = end,
                AssignedTrainerId = trainerId
            };
        }

        public void Extend(DateTime newEndDate)
        {
            if (newEndDate <= EndDate)
                throw new InvalidOperationException("Yeni tarix əvvəlkindən uzun olmalıdır.");

            EndDate = newEndDate;
        }

        public void AssignTrainer(Guid trainerId)
        {
            AssignedTrainerId = trainerId;
        }

    }
}
