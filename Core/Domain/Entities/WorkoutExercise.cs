using Domain.Common;

namespace Domain.Entities
{
    public class WorkoutExercise : BaseAuditableEntity
    {
        public Guid WorkoutId { get; private set; }
        public Workout Workout { get; private set; } = null!;

        public string ExerciseName { get; private set; } = string.Empty;
        public int Sets { get; private set; }
        public int Reps { get; private set; }
        public decimal? Weight { get; private set; } // in kg
        public int? RestTimeSeconds { get; private set; }
        public string? Instructions { get; private set; }
        public bool IsCompleted { get; private set; }

        private WorkoutExercise() { }

        public static WorkoutExercise Create(Guid workoutId, string exerciseName, int sets, int reps)
        {
            return new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                WorkoutId = workoutId,
                ExerciseName = exerciseName.Trim(),
                Sets = sets,
                Reps = reps
            };
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
    }
}
