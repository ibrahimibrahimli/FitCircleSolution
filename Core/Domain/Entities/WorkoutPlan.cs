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

    // Supporting entities for DietPlan
    public class DietPlanMeal : BaseAuditableEntity
    {
        public Guid DietPlanId { get; private set; }
        public DietPlan DietPlan { get; private set; } = null!;

        public string MealName { get; private set; } = string.Empty; // Breakfast, Lunch, Dinner, Snack
        public TimeSpan RecommendedTime { get; private set; }
        public string Foods { get; private set; } = string.Empty; // JSON or comma-separated
        public decimal? EstimatedCalories { get; private set; }
        public string? Instructions { get; private set; }

        private DietPlanMeal() { }

        public static DietPlanMeal Create(Guid dietPlanId, string mealName, TimeSpan recommendedTime, string foods)
        {
            return new DietPlanMeal
            {
                Id = Guid.NewGuid(),
                DietPlanId = dietPlanId,
                MealName = mealName.Trim(),
                RecommendedTime = recommendedTime,
                Foods = foods.Trim()
            };
        }
    }

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

    // Supporting entities for WorkoutPlan
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

namespace Domain.Enums
{
    public enum DietGoal
    {
        WeightLoss = 1,
        WeightGain = 2,
        WeightMaintenance = 3,
        MuscleGain = 4,
        FatLoss = 5,
        HealthImprovement = 6,
        SportsPerformance = 7
    }

    public enum DietType
    {
        Balanced = 1,
        LowCarb = 2,
        LowFat = 3,
        HighProtein = 4,
        Keto = 5,
        Vegetarian = 6,
        Vegan = 7,
        Mediterranean = 8,
        Paleo = 9
    }

    public enum WorkoutGoal
    {
        WeightLoss = 1,
        MuscleGain = 2,
        StrengthBuilding = 3,
        Endurance = 4,
        GeneralFitness = 5,
        Flexibility = 6,
        SportsSpecific = 7,
        Rehabilitation = 8
    }

    public enum FitnessLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3,
        Expert = 4
    }
}

namespace Domain.Extensions
{
    public static class DietGoalExtensions
    {
        public static string GetDisplayName(this DietGoal goal)
        {
            return goal switch
            {
                DietGoal.WeightLoss => "Arıqlama",
                DietGoal.WeightGain => "Kilo Artırma",
                DietGoal.WeightMaintenance => "Çəki Saxlama",
                DietGoal.MuscleGain => "Əzələ Artırma",
                DietGoal.FatLoss => "Yağ Azaltma",
                DietGoal.HealthImprovement => "Sağlamlıq Yaxşılaşdırma",
                DietGoal.SportsPerformance => "İdman Performansı",
                _ => goal.ToString()
            };
        }
    }

    public static class WorkoutGoalExtensions
    {
        public static string GetDisplayName(this WorkoutGoal goal)
        {
            return goal switch
            {
                WorkoutGoal.WeightLoss => "Arıqlama",
                WorkoutGoal.MuscleGain => "Əzələ Artırma",
                WorkoutGoal.StrengthBuilding => "Güc Artırma",
                WorkoutGoal.Endurance => "Dayanıqlıq",
                WorkoutGoal.GeneralFitness => "Ümumi Fitnes",
                WorkoutGoal.Flexibility => "Çeviklik",
                WorkoutGoal.SportsSpecific => "İdman Spesifik",
                WorkoutGoal.Rehabilitation => "Reabilitasiya",
                _ => goal.ToString()
            };
        }
    }

    public static class FitnessLevelExtensions
    {
        public static string GetDisplayName(this FitnessLevel level)
        {
            return level switch
            {
                FitnessLevel.Beginner => "Başlanğıc",
                FitnessLevel.Intermediate => "Orta",
                FitnessLevel.Advanced => "İrəliləmiş",
                FitnessLevel.Expert => "Ekspert",
                _ => level.ToString()
            };
        }
    }
}