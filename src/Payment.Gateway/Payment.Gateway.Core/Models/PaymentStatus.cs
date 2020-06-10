namespace Payment.Gateway.Core.Models
{
    public enum PaymentStatus
    {
        Unknown = 0,
        Success = 1,
        Pending = 2,
        Declined = 3,
        Error = 4,
        PartnerError = 5
    }
}
