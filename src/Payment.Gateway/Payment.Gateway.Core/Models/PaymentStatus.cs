namespace Payment.Gateway.Core.Models
{
    public enum PaymentStatus
    {
        Unknown = 0,
        Success = 1,
        Declined = 2,
        PartnerError = 3
    }
}
