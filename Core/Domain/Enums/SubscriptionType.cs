namespace Domain.Enums
{
    public enum SubscriptionType
    {
        Basic = 1,
        Premium = 2,
        Vip = 3,
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