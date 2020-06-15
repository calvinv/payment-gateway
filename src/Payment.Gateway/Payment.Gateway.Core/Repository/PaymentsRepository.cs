using Dapper;
using Payment.Gateway.Core.Models;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Core.Configuration;
using Microsoft.Extensions.Options;
using Payment.Gateway.Core.Models.Dto;

namespace Payment.Gateway.Core.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private const string UpdateCardPaymentProcedure = "UpdateCardPayment";
        private const string CreateCardPaymentProcedure = "CreateCardPayment";
        private const string GetCardPaymentByReferenceProcedure = "GetCardPaymentByReference";

        private readonly string _connectionString;

        public PaymentsRepository(ILogger<PaymentsRepository> logger, IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task CreatePaymentRequest(CardPayment cardPayment)
        {
            var cardPaymentDto = new CardPaymentDto()
            {
                Amount = cardPayment.PaymentAmount.Amount,
                CardNumber = cardPayment.CardDetails.CardNumber,
                Currency = cardPayment.PaymentAmount.Currency,
                Cvv = cardPayment.CardDetails.Cvv,
                ExpiryMonth = cardPayment.CardDetails.ExpiryMonth,
                ExpiryYear = cardPayment.CardDetails.ExpiryYear,
                CustomerName = cardPayment.CustomerDetails.CustomerName,
                CustomerAddress = cardPayment.CustomerDetails.CustomerAddress,
                Reference = cardPayment.Reference,
                ThirdPartyReference = "",
                PaymentStatus = PaymentStatus.Pending,
                 
            };

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(CreateCardPaymentProcedure,
                cardPaymentDto, 
                commandType: CommandType.StoredProcedure);
        }
        
        public async Task<CardPayment> GetCardPayment(string reference)
        {
            using var connection = new SqlConnection(_connectionString);
            var cardPaymentDto = await connection.QuerySingleOrDefaultAsync<CardPaymentDto>(GetCardPaymentByReferenceProcedure, new { reference }, commandType: CommandType.StoredProcedure);

            return new CardPayment()
            {
                Reference = cardPaymentDto.Reference,
                PaymentStatus = cardPaymentDto.PaymentStatus,
                CardDetails = new CardDetails()
                {
                    CardNumber = cardPaymentDto.CardNumber,
                    Cvv = cardPaymentDto.Cvv,
                    ExpiryMonth = cardPaymentDto.ExpiryMonth,
                    ExpiryYear = cardPaymentDto.ExpiryYear
                },
                CustomerDetails = new CustomerDetails()
                {
                    CustomerAddress = cardPaymentDto.CustomerAddress,
                    CustomerName = cardPaymentDto.CustomerName
                },
                PaymentAmount = new PaymentAmount()
                {
                    Amount = cardPaymentDto.Amount,
                    Currency = cardPaymentDto.Currency
                }                
            };
        }

        public async Task UpdatePaymentResult(PaymentResult paymentResult)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(UpdateCardPaymentProcedure, new { paymentResult.Reference, paymentResult.ThirdPartyReference, paymentResult.PaymentStatus }, commandType: CommandType.StoredProcedure);
        }
    }
}
