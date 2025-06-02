using Domain.Common;

namespace Domain.Entities
{
    public class DietPlanProgress : BaseAuditableEntity
    {
        public Guid DietPlanId { get; private set; }
        public DietPlan DietPlan { get; private set; } = null!;

        public DateTime RecordDate { get; private set; }
        public decimal? ActualCalories { get; private set; }
        public decimal? ActualProtein { get; private set; }
        public decimal? ActualCarbs { get; private set; }
        public decimal? ActualFats { get; private set; }
        public decimal? Weight { get; private set; }
        public string? Notes { get; private set; }

        private DietPlanProgress() { }

        public static DietPlanProgress Create(Guid dietPlanId, DateTime recordDate)
        {
            return new DietPlanProgress
            {
                Id = Guid.NewGuid(),
                DietPlanId = dietPlanId,
                RecordDate = recordDate.Date
            };
        }
    }
}
