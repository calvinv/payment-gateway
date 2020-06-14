using Payment.Gateway.Core.Models;
using System.Threading.Tasks;

namespace Payment.Gateway.Core.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        public Task CreatePaymentRequest(CardPayment cardPayment)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdatePaymentResult(PaymentResult paymentresult)
        {
            throw new System.NotImplementedException();
        }
    }
}
