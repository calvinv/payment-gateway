using System;

namespace Payment.Gateway.Core.Models.FoobarBank
{
    public class PaymentResponse
    {
        public DateTime TimeStamp { get; set; }

        public string PaymentStatus { get; set; }

        public string Reference { get; set; }
    }
}
