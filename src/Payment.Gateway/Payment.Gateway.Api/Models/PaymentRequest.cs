using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Gateway.Api.Models
{
    public class PaymentRequest
    {
        //
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }

    }
}
