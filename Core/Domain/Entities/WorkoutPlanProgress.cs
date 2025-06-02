using Domain.Common;

namespace Domain.Entities
{
    public class WorkoutPlanProgress : BaseAuditableEntity
    {
        public Guid WorkoutPlanId { get; private set; }
        public WorkoutPlan WorkoutPlan { get; private set; } = null!;

        public DateTime RecordDate { get; private set; }
        public decimal? Weight { get; private set; }
        public decimal? BodyFatPercentage { get; private set; }
        public decimal? MusclePercentage { get; private set; }
        public string? Measurements { get; private set; } // JSON format for various body measurements
        public string? Notes { get; private set; }

        private WorkoutPlanProgress() { }

        public static WorkoutPlanProgress Create(Guid workoutPlanId, DateTime recordDate)
        {
            return new WorkoutPlanProgress
            {
                Id = Guid.NewGuid(),
                WorkoutPlanId = workoutPlanId,
                RecordDate = recordDate.Date
            };
        }
    }
}
