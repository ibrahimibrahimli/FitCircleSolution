using Domain.Enums;

namespace Domain.Common.Helpers
{
    public static class SubcriptionTypeHelper
    {
        public static SubscriptionType DetermineSubcriptionType(decimal monthlyPrice)
        {
            return monthlyPrice switch
            {
                < 40 => throw new ArgumentException("The minimum price must be 40 AZN."),
                >= 40 and < 60 => SubscriptionType.Basic,
                >= 60 and < 80 => SubscriptionType.Premium,
                >= 80 and <= 100 => SubscriptionType.Vip,
                > 100 => SubscriptionType.Vip,
            };
        }
    }
}
