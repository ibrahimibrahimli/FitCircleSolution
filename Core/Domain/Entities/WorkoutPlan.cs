using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class WorkoutPlan : BaseAuditableEntity
    {
        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public Guid? CreatedByTrainerId { get; private set; }

        public Trainer? CreatedByTrainer { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public WorkoutGoal Goal { get; private set; }

        public FitnessLevel Level { get; private set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate && !IsCompleted;

        public bool IsCompleted { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public int WorkoutsPerWeek { get; private set; } = 3;

        public int EstimatedDurationMinutes { get; private set; } = 60;

        public string? Equipment { get; private set; }

        public string? SpecialInstructions { get; private set; }

        public string? Restrictions { get; private set; } // Injuries, limitations

        public bool IsCustomPlan { get; private set; }

        public int RemainingDays => IsActive ? Math.Max(0, (EndDate.Date - DateTime.UtcNow.Date).Days) : 0;

        public decimal CompletionPercentage
        {
            get
            {
                if (!Workouts.Any()) return 0;
                var completedWorkouts = Workouts.Count(w => w.IsCompleted);
                return Math.Round((decimal)completedWorkouts / Workouts.Count * 100, 2);
            }
        }

        public ICollection<Workout> Workouts { get; private set; } = new List<Workout>();

        public ICollection<WorkoutPlanProgress> ProgressRecords { get; private set; } = new List<WorkoutPlanProgress>();

        // Private constructor for EF Core
        private WorkoutPlan() { }

        public static WorkoutPlan Create(
            string name,
            Guid userId,
            DateTime startDate,
            DateTime endDate,
            WorkoutGoal goal,
            FitnessLevel level,
            Guid? createdByTrainerId = null,
            string? description = null,
            bool isCustomPlan = false)
        {
            ValidatePlanDates(startDate, endDate);
            ValidateName(name);

            return new WorkoutPlan
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description?.Trim(),
                UserId = userId,
                CreatedByTrainerId = createdByTrainerId,
                StartDate = startDate,
                EndDate = endDate,
                Goal = goal,
                Level = level,
                IsCustomPlan = isCustomPlan,
                IsCompleted = false
            };
        }

        public void UpdateBasicInfo(string name, string? description = null)
        {
            ValidateName(name);
            Name = name.Trim();
            Description = description?.Trim();
        }

        public void UpdateDates(DateTime newStartDate, DateTime newEndDate)
        {
            ValidatePlanDates(newStartDate, newEndDate);
            StartDate = newStartDate;
            EndDate = newEndDate;
        }

        public void SetWorkoutFrequency(int workoutsPerWeek, int estimatedDurationMinutes)
        {
            if (workoutsPerWeek < 1 || workoutsPerWeek > 7)
                throw new ArgumentException("Həftəlik məşq sayı 1-7 arasında olmalıdır.");

            if (estimatedDurationMinutes < 15 || estimatedDurationMinutes > 300)
                throw new ArgumentException("Məşq müddəti 15-300 dəqiqə arasında olmalıdır.");

            WorkoutsPerWeek = workoutsPerWeek;
            EstimatedDurationMinutes = estimatedDurationMinutes;
        }

        public void SetEquipment(string? equipment)
        {
            Equipment = equipment?.Trim();
        }

        public void SetSpecialInstructions(string? instructions)
        {
            SpecialInstructions = instructions?.Trim();
        }

        public void SetRestrictions(string? restrictions)
        {
            Restrictions = restrictions?.Trim();
        }

        public void UpdateGoalAndLevel(WorkoutGoal goal, FitnessLevel level)
        {
            Goal = goal;
            Level = level;
        }

        public void Complete()
        {
            if (IsCompleted)
                throw new InvalidOperationException("Plan artıq tamamlanıb.");

            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void Reopen()
        {
            if (!IsCompleted)
                throw new InvalidOperationException("Plan artıq açıqdır.");

            IsCompleted = false;
            CompletedAt = null;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Plan adı boş ola bilməz.");

            if (name.Trim().Length < 3)
                throw new ArgumentException("Plan adı ən azı 3 simvol olmalıdır.");
        }

        private static void ValidatePlanDates(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Başlama tarixi bitmə tarixindən əvvəl olmalıdır.");

            if (endDate < DateTime.Today)
                throw new ArgumentException("Bitmə tarixi bugünkü tarixdən sonra olmalıdır.");
        }
    }
}