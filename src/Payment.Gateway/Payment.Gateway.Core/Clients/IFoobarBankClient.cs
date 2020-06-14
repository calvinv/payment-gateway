using Payment.Gateway.Core.Models;
using System.Threading.Tasks;

namespace Payment.Gateway.Core.Clients
{
    public interface IFoobarBankClient
    {
        Task<PaymentResult> CreatePayment(CardPayment cardPayment);
        Task<AuthenticationResult> GetAuthenticationToken();
    }
}