using Domain.Common;
using Domain.Enums;
using Domain.Identity;

namespace Domain.Enums
{
    public enum SubscriptionType
    {
        Basic = 1,
        Premium = 2,
        VIP = 3,
        Trial = 4,
        Corporate = 6
    }

  
    public enum RefundReason
    {
        CustomerRequest = 1,
        ServiceNotProvided = 2,
        TechnicalIssue = 3,
        Duplicate = 4,
        Fraud = 5,
        Other = 6
    }
}