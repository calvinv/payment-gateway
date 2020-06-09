using System;

namespace Bank.Simulator.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
    }
}
