using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Core.Models.Dto
{
    public class CardPaymentDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Reference { get; set; }
        public string ThirdPartyReference { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}
