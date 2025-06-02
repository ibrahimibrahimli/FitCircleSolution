using Domain.Common;

namespace Domain.Entities
{
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
}
