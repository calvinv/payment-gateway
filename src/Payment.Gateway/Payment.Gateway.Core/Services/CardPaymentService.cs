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
        private readonly IFoobarBankClient _foobarBankClient;
        private readonly IPaymentsRepository _paymentsRepository;

        public CardPaymentService(IFoobarBankClient foobarBankClient, IPaymentsRepository paymentsRepository)
        {
            _foobarBankClient = foobarBankClient;
            _paymentsRepository = paymentsRepository;
        }

        public async Task<PaymentResult> CreateCardPayment(CardPayment cardPayment)
        {
            await _paymentsRepository.CreatePaymentRequest(cardPayment);
            var paymentresult = await _foobarBankClient.CreatePayment(cardPayment);
            await _paymentsRepository.UpdatePaymentResult(paymentresult);

            return paymentresult;
        }

    }
}
