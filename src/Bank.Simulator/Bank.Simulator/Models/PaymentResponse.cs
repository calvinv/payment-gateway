using System;

namespace Bank.Simulator.Models
{
    public class PaymentResponse
    {
        public DateTime TimeStamp { get; set; }

        public string PaymentStatus { get; set; }

        public string Reference { get; set; }
        public string FoobarReference { get; set; }
    }
}
