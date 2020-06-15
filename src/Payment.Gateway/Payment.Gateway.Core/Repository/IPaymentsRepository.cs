using Payment.Gateway.Core.Models;
using System.Threading.Tasks;

namespace Payment.Gateway.Core.Repository
{
    public interface IPaymentsRepository
    {
        Task CreatePaymentRequest(CardPayment cardPayment);
        Task UpdatePaymentResult(PaymentResult paymentresult);
        Task<CardPayment> GetCardPayment(string reference);
    }
}