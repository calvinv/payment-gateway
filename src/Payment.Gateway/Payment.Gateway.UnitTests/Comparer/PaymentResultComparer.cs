using Payment.Gateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Payment.Gateway.UnitTests.Comparer
{
    public class PaymentResultComparer : IEqualityComparer<PaymentResult>
    {
        public bool Equals([AllowNull] PaymentResult x, [AllowNull] PaymentResult y)
        {
            if (x == null && y == null)
                return true;
            if (x != null && y == null)
                return false;
            if (x == null && y != null)
                return false;

            if (x.PaymentStatus == y.PaymentStatus &&
                x.Reference == y.Reference &&
                x.ThirdPartyReference == y.ThirdPartyReference)
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] PaymentResult obj)
        {
            return obj.GetHashCode();
        }
    }
}
