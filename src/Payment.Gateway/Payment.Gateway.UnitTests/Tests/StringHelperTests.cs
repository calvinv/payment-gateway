using Payment.Gateway.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Xunit;

namespace Payment.Gateway.UnitTests.Tests
{
    public class StringHelperTests
    {
        [Fact]
        public void ShouldCensorCreditCardNumber()
        {
            var creditCardNumber = "5555 3412 4444 1115";
            var expectedNumber = "***************1115";

            var actual = StringHelper.HideCardNumber(creditCardNumber);

            Assert.Equal(expectedNumber, actual);
        }
    }
}
