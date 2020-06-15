using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Api.Models;
using Payment.Gateway.Core.Models;
using Payment.Gateway.Core.Services;

namespace Payment.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardPaymentsController : ControllerBase
    {
        private readonly ICardPaymentService _cardPaymentService;
        private readonly ILogger<CardPaymentsController> _logger;
        private readonly IAuthenticationService _authorizationService;

        public CardPaymentsController(ICardPaymentService cardPaymentService, ILogger<CardPaymentsController> logger, IAuthenticationService authorizationService)
        {
            _cardPaymentService = cardPaymentService;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        [HttpGet("{reference}")]
        public async Task<IActionResult> GetPayment([FromRoute]string reference)
        {            
            if (string.IsNullOrEmpty(Request.Headers["x-api-key"]) || (!_authorizationService.ValidateToken(Request.Headers["x-api-key"])))
            {
                return Unauthorized();
            }

            var result = await _cardPaymentService.GetCardPaymentByReference(reference);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentRequest cardPaymentRequest)
        {
            if (string.IsNullOrEmpty(Request.Headers["x-api-key"]) || (!_authorizationService.ValidateToken(Request.Headers["x-api-key"])))
            {
                return Unauthorized();
            }          

            var cardPayment = new CardPayment()
            {
                CardDetails = new CardDetails
                {
                    CardNumber = cardPaymentRequest.CardNumber,
                    Cvv = cardPaymentRequest.Cvv,
                    ExpiryMonth = cardPaymentRequest.ExpiryMonth,
                    ExpiryYear = cardPaymentRequest.ExpiryYear                    
                },
                CustomerDetails = new CustomerDetails()
                {
                    CustomerAddress = cardPaymentRequest.CustomerAddress,
                    CustomerName = cardPaymentRequest.CustomerName                    
                },
                PaymentAmount = new PaymentAmount()
                {
                    Amount = cardPaymentRequest.Amount,
                    Currency = cardPaymentRequest.Currency
                },
                Reference = cardPaymentRequest.Reference              
            };

            var paymentResult = await _cardPaymentService.CreateCardPayment(cardPayment);

            switch (paymentResult.PaymentStatus)
            {
                case PaymentStatus.Success:
                case PaymentStatus.Declined:
                    return Created(paymentResult.Reference, paymentResult);
                case PaymentStatus.Pending:
                    return Accepted(paymentResult);
                case PaymentStatus.PartnerError:
                    return StatusCode(StatusCodes.Status502BadGateway, paymentResult);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, paymentResult);                    
            }
        }
    }
}
