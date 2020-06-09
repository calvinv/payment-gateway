using System;

namespace Bank.Simulator.Models
{
    public class PaymentResponse
    {
        public DateTime TimeStamp { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string Reference { get; set; }
    }
}
