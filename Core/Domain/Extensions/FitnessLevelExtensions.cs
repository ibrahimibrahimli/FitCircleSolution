using Domain.Enums;

namespace Domain.Extensions
{
    public static class FitnessLevelExtensions
    {
        public static string GetDisplayName(this FitnessLevel level)
        {
            return level switch
            {
                FitnessLevel.Beginner => "Beginner",
                FitnessLevel.Intermediate => "Intermediate",
                FitnessLevel.Advanced => "Advanced",
                FitnessLevel.Expert => "Expert",
                _ => level.ToString()
            };
        }
    }
}
