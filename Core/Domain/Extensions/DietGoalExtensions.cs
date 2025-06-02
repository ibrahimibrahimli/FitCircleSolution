using Domain.Enums;

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
}
