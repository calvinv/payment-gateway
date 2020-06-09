using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Simulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Simulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController()
        {

        }

        [HttpPost]
        public PaymentResponse CreatePayment([FromHeader(x-api-key)]string authorization,PaymentRequest paymentRequest)
        {
            // could read the payment request and return different responses
            return new PaymentResponse()
            {
                PaymentStatus = PaymentStatus.Success,
                Reference = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}
