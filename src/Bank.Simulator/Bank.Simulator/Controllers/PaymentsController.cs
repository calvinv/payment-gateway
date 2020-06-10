using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bank.Simulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Simulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController()
        {

        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public IActionResult CreatePayment([FromBody]PaymentRequest paymentRequest)
        {
            if (string.IsNullOrEmpty(Request.Headers["Authorization"]))
            {
                return Unauthorized(new ErrorResponse() {Message = "Please add authorization to the request"});
            }            

            return Ok(new PaymentResponse()
            {
                PaymentStatus = PaymentStatus.Success.ToString(),
                Reference = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.UtcNow
            });
        }
    }
}
