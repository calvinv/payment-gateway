using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Payment.Gateway.Core.Clients;
using Payment.Gateway.Core.Models;
using Payment.Gateway.Core.Repository;
using Payment.Gateway.Core.Services;
using Payment.Gateway.UnitTests.Builders;
using Payment.Gateway.UnitTests.Comparer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Payment.Gateway.UnitTests.Tests
{
    public class CardPaymentServiceTests
    {
        private readonly PaymentResultComparer _paymentResultComparer;
        private readonly IFoobarBankClient _foobarBankClient;
        public readonly IPaymentsRepository _paymentsRepository;
        private readonly ILogger<CardPaymentService> _logger;
        private readonly CardPaymentService _cardPaymentService;

        public CardPaymentServiceTests()
        {
            _paymentResultComparer = new PaymentResultComparer();
            _foobarBankClient = Substitute.For<IFoobarBankClient>();
            _paymentsRepository = Substitute.For<IPaymentsRepository>();
            _logger = Substitute.For<ILogger<CardPaymentService>>();
            _cardPaymentService = new CardPaymentService(_logger, _foobarBankClient, _paymentsRepository);
        }

        [Theory]
        [InlineData(PaymentStatus.Unknown)]
        [InlineData(PaymentStatus.Declined)]
        [InlineData(PaymentStatus.Error)]
        [InlineData(PaymentStatus.PartnerError)]
        [InlineData(PaymentStatus.Pending)]
        [InlineData(PaymentStatus.Success)]
        public async Task ShouldProcessPaymentWhenPartnerBankRespondsForAllStatuses(PaymentStatus paymentStatus)
        {
            var cardPayment = new CardPaymentBuilder().Build();
            var successfulPayment = new PaymentResult()
            {
                PaymentStatus = paymentStatus,
                Reference = cardPayment.Reference,
                ThirdPartyReference = "12345"
            };

            _foobarBankClient.CreatePayment(cardPayment).Returns(successfulPayment);

            var result = await _cardPaymentService.CreateCardPayment(cardPayment);

            await _paymentsRepository.Received(1).CreatePaymentRequest(cardPayment);
            await _paymentsRepository.Received(1).UpdatePaymentResult(successfulPayment);

            Assert.Equal(successfulPayment, result, _paymentResultComparer);
        }

        [Fact]
        public async Task ShouldReturnPaymentErrorWhenDatabaseThrows()
        {
            var cardPayment = new CardPaymentBuilder().Build();
            _paymentsRepository.CreatePaymentRequest(cardPayment).Throws(new Exception());

            var result = await _cardPaymentService.CreateCardPayment(cardPayment);

            await _paymentsRepository.Received(1).CreatePaymentRequest(cardPayment);

            Assert.Equal(PaymentStatus.Error, result.PaymentStatus);
            Assert.Equal(cardPayment.Reference, result.Reference);
        }

        [Fact]
        public async Task ShouldReturnPaymentErrorWhenFoobarBankClientThrows()
        {
            var cardPayment = new CardPaymentBuilder().Build();

            _foobarBankClient.CreatePayment(cardPayment).Throws(new Exception());

            var result = await _cardPaymentService.CreateCardPayment(cardPayment);

            await _paymentsRepository.Received(1).CreatePaymentRequest(cardPayment);

            Assert.Equal(PaymentStatus.Error, result.PaymentStatus);
            Assert.Equal(cardPayment.Reference, result.Reference);
        }
    }
}
