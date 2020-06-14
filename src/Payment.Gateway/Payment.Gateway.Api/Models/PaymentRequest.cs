using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Gateway.Api.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }

    }

}
