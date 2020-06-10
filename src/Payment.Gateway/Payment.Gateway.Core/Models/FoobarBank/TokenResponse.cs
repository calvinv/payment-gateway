using System;

namespace Payment.Gateway.Core.Models.FoobarBank
{
    public class TokenResponse
    { 
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
