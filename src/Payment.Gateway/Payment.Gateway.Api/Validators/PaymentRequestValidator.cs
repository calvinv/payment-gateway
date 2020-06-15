using FluentValidation;
using Microsoft.VisualBasic;
using Payment.Gateway.Api.Models;
using System;
using System.Globalization;

namespace Payment.Gateway.Api.Validators
{
	public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
	{
		public PaymentRequestValidator()
		{
			var todaysYear = DateTime.Now.Year % 2000;
			var twentyYearsFromNow = DateTime.Now.AddYears(20).Year % 2000;


			RuleFor(x => x.Amount).ScalePrecision(2, 15);
			RuleFor(x => x.CardNumber).CreditCard();
			RuleFor(x => x.Currency).Length(3);
			RuleFor(x => x.CustomerAddress).NotEmpty();
			RuleFor(x => x.CustomerName).NotEmpty();
			RuleFor(x => x.ExpiryMonth).InclusiveBetween(1, 12);
			RuleFor(x => x.ExpiryYear).InclusiveBetween(todaysYear, twentyYearsFromNow);
			RuleFor(x => x.Cvv).Length(3, 5);			
			RuleFor(x => x.Reference).NotNull();
			RuleFor(x => x).Must(HaveExpiryDateInTheFuture);
		}

		private bool HaveExpiryDateInTheFuture(PaymentRequest paymentRequest)
		{
			var year = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(paymentRequest.ExpiryYear);
			var expiryDate = new DateTime(year, paymentRequest.ExpiryMonth, 1);
			return expiryDate > DateTime.Now;
		}
	}
}
