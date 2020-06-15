using Payment.Gateway.Core.Models;
using System.Threading.Tasks;

namespace Payment.Gateway.Core.Services
{
    public interface ICardPaymentService
    {
        Task<PaymentResult> CreateCardPayment(CardPayment cardPayment);
        Task<CardPayment> GetCardPaymentByReference(string reference);
    }
}