using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class DietPlan : BaseAuditableEntity
    {
        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public Guid? CreatedByTrainerId { get; private set; }

        public Trainer? CreatedByTrainer { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public DietGoal Goal { get; private set; }

        public DietType Type { get; private set; }

        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate && !IsCompleted;

        public bool IsCompleted { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public decimal? TargetCalories { get; private set; }

        public decimal? TargetProtein { get; private set; } // in grams

        public decimal? TargetCarbs { get; private set; } // in grams

        public decimal? TargetFats { get; private set; } // in grams

        public string? SpecialInstructions { get; private set; }

        public string? Restrictions { get; private set; } // Allergies, dietary restrictions

        public int MealsPerDay { get; private set; } = 3;

        public bool IsCustomPlan { get; private set; }

        public int RemainingDays => IsActive ? Math.Max(0, (EndDate.Date - DateTime.UtcNow.Date).Days) : 0;

        public ICollection<DietPlanMeal> Meals { get; private set; } = new List<DietPlanMeal>();

        public ICollection<DietPlanProgress> ProgressRecords { get; private set; } = new List<DietPlanProgress>();

        // Private constructor for EF Core
        private DietPlan() { }

        public static DietPlan Create(
            string name,
            Guid userId,
            DateTime startDate,
            DateTime endDate,
            DietGoal goal,
            DietType type,
            Guid? createdByTrainerId = null,
            string? description = null,
            bool isCustomPlan = false)
        {
            ValidatePlanDates(startDate, endDate);
            ValidateName(name);

            return new DietPlan
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description?.Trim(),
                UserId = userId,
                CreatedByTrainerId = createdByTrainerId,
                StartDate = startDate,
                EndDate = endDate,
                Goal = goal,
                Type = type,
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

        public void SetNutritionTargets(
            decimal? targetCalories = null,
            decimal? targetProtein = null,
            decimal? targetCarbs = null,
            decimal? targetFats = null)
        {
            if (targetCalories.HasValue && targetCalories <= 0)
                throw new ArgumentException("Hədəf kalori sıfırdan böyük olmalıdır.");

            if (targetProtein.HasValue && targetProtein < 0)
                throw new ArgumentException("Hədəf protein mənfi ola bilməz.");

            if (targetCarbs.HasValue && targetCarbs < 0)
                throw new ArgumentException("Hədəf karbohidrat mənfi ola bilməz.");

            if (targetFats.HasValue && targetFats < 0)
                throw new ArgumentException("Hədəf yağ mənfi ola bilməz.");

            TargetCalories = targetCalories;
            TargetProtein = targetProtein;
            TargetCarbs = targetCarbs;
            TargetFats = targetFats;
        }

        public void SetMealsPerDay(int mealsCount)
        {
            if (mealsCount < 1 || mealsCount > 8)
                throw new ArgumentException("Günlük yemək sayı 1-8 arasında olmalıdır.");

            MealsPerDay = mealsCount;
        }

        public void SetSpecialInstructions(string? instructions)
        {
            SpecialInstructions = instructions?.Trim();
        }

        public void SetRestrictions(string? restrictions)
        {
            Restrictions = restrictions?.Trim();
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