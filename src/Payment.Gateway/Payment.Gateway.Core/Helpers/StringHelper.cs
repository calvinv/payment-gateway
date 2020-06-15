using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Core.Helpers
{
    public static class StringHelper
    {        
        public static string HideCardNumber(string cardNumber)
        {
            const int digitsVisible = 4;

            var sb = new StringBuilder();

            for (int j=0; j<cardNumber.Length; j++)
            {
                if (j > ((cardNumber.Length - 1) - digitsVisible))
                {
                    sb.Append(cardNumber[j]);
                }
                else
                {
                    sb.Append('*');
                }
            }

            return sb.ToString();
        }
    }
}
