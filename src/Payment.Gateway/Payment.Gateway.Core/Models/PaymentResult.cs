namespace Payment.Gateway.Core.Models
{
    public class PaymentResult
    {
        public PaymentStatus PaymentStatus { get; set; }
        public string Reference { get; set; }
        public string ThirdPartyReference { get; set; }
    }
}
