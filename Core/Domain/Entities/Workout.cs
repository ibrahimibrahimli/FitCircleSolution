using Domain.Common;

namespace Domain.Entities
{
    public class Workout : BaseAuditableEntity
    {
        public Guid WorkoutPlanId { get; private set; }
        public WorkoutPlan WorkoutPlan { get; private set; } = null!;

        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public DateTime ScheduledDate { get; private set; }
        public int EstimatedDurationMinutes { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public int? ActualDurationMinutes { get; private set; }
        public string? CompletionNotes { get; private set; }

        public ICollection<WorkoutExercise> Exercises { get; private set; } = new List<WorkoutExercise>();

        private Workout() { }

        public static Workout Create(Guid workoutPlanId, string name, DateTime scheduledDate, int estimatedDurationMinutes)
        {
            return new Workout
            {
                Id = Guid.NewGuid(),
                WorkoutPlanId = workoutPlanId,
                Name = name.Trim(),
                ScheduledDate = scheduledDate,
                EstimatedDurationMinutes = estimatedDurationMinutes
            };
        }

        public void MarkAsCompleted(int actualDurationMinutes, string? notes = null)
        {
            if (IsCompleted)
                throw new InvalidOperationException("Məşq artıq tamamlanıb.");

            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
            ActualDurationMinutes = actualDurationMinutes;
            CompletionNotes = notes?.Trim();
        }
    }
}
