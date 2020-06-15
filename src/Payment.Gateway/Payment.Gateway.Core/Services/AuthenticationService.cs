using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Gateway.Core.Configuration;
using System;
using System.Globalization;
using System.Linq;

namespace Payment.Gateway.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string BearerPrefix = "bearer";
        private readonly string _apiKey;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ILogger<AuthenticationService> logger, IOptions<AuthenticationServiceOptions> options)
        {
            _apiKey = options.Value.ApiKey;
            _logger = logger;
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            
            if (token != _apiKey)
            {
                _logger.LogWarning("Invalid apikey used for authentication");
                return false;
            }

            return true;
        }

    }
}
