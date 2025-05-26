using Domain.Enums;

namespace Domain.Common.Helpers
{
    public static class SubcriptionTypeHelper
    {
        public static SubcriptionType DetermineSubcriptionType(decimal monthlyPrice)
        {
            return monthlyPrice switch
            {
                < 40 => throw new ArgumentException("The minimum price must be 40 AZN."),
                >= 40 and < 60 => SubcriptionType.Standart,
                >= 60 and < 80 => SubcriptionType.Gold,
                >= 80 and <= 100 => SubcriptionType.Premium,
                > 100 => SubcriptionType.Vip,
                _ => throw new InvalidOperationException("No matching price range found.")
            };
        }
    }
}
