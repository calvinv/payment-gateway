using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Gateway.Core.Models
{
    public class AuthenticationResult
    {
        public bool IsSuccessful { get; set; }
        public string Token { get; set; }
    }
}
