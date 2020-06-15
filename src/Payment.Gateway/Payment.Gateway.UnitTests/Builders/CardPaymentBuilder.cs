using NSubstitute.Routing.Handlers;
using Payment.Gateway.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Payment.Gateway.UnitTests.Builders
{
    public class CardPaymentBuilder
    {
        private CardPayment _cardPayment;

        public CardPaymentBuilder()
        {
            _cardPayment = new CardPayment()
            {
                CardDetails = new CardDetails()
                {
                    CardNumber = "12345678910000",
                    Cvv = "123",
                    ExpiryMonth = 1,
                    ExpiryYear = 25
                },
                CustomerDetails = new CustomerDetails()
                {
                    CustomerAddress = "123 Lane Road, London, W1 1AA, UK",
                    CustomerName = "Bob Roberts"
                },
                PaymentAmount = new PaymentAmount()
                {
                    Amount = 800,
                    Currency = "GBP"
                },
                Reference = Guid.NewGuid().ToString()
            };
        }

        public CardPayment Build()
        {
            return _cardPayment;
        }
    }
}
