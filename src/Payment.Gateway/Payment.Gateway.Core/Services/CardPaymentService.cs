using Microsoft.Extensions.Logging;
using Payment.Gateway.Core.Clients;
using Payment.Gateway.Core.Models;
using Payment.Gateway.Core.Repository;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Payment.Gateway.Core.Services
{
    public class CardPaymentService : ICardPaymentService
    {
        private readonly ILogger<CardPaymentService> _logger;
        private readonly IFoobarBankClient _foobarBankClient;
        private readonly IPaymentsRepository _paymentsRepository;

        public CardPaymentService(ILogger<CardPaymentService> logger, IFoobarBankClient foobarBankClient, IPaymentsRepository paymentsRepository)
        {
            _logger = logger;
            _foobarBankClient = foobarBankClient;
            _paymentsRepository = paymentsRepository;
        }

        public async Task<PaymentResult> CreateCardPayment(CardPayment cardPayment)
        {
            var result = new PaymentResult();
            try
            {
                await _paymentsRepository.CreatePaymentRequest(cardPayment);
                result = await _foobarBankClient.CreatePayment(cardPayment);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred creating card payment Reference={cardPayment.Reference}");
                result = new PaymentResult()
                {
                    PaymentStatus = PaymentStatus.Error,
                    Reference = cardPayment.Reference
                };
                return result;
            }
            finally
            {
                await _paymentsRepository.UpdatePaymentResult(result);
            }
        }
    }
}
