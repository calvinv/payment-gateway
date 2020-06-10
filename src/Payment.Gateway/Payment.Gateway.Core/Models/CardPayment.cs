using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Core.Models
{
    public class CardPayment
    {
        public string Reference { get; set; }
        public PaymentAmount PaymentAmount { get; set; }
        public CardDetails CardDetails { get; set; }
        public CustomerDetails CustomerDetails { get; set; }
    }
}
