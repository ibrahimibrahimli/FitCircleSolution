using Domain.Enums;

namespace Domain.Extensions
{
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
}
