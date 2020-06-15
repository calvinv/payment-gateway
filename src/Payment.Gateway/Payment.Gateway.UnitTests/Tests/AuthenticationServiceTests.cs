using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Payment.Gateway.Core.Services;
using Payment.Gateway.Core.Configuration;
using Xunit;

namespace Payment.Gateway.UnitTests.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authorizationService;
        private const string _apiKey = "secret";

        public AuthenticationServiceTests()
        {
            var logger = Substitute.For<ILogger<AuthenticationService>>();
            
            var options = Substitute.For<IOptions<AuthenticationServiceOptions>>();
            var optionsValue = new AuthenticationServiceOptions()
            {
                ApiKey = _apiKey
            };
            options.Value.Returns(optionsValue);

            _authorizationService = new AuthenticationService(logger, options);
        }

        [Fact]
        public void ShouldReturnTrueWhenApiKeyCorrect()
        {
            var result = _authorizationService.ValidateToken(_apiKey);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenApiKeyIncorrect()
        {
            var result = _authorizationService.ValidateToken("notthesecret");

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenApiKeyEmpty()
        {
            var result = _authorizationService.ValidateToken("");

            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenApiKeyNull()
        {
            var result = _authorizationService.ValidateToken(null);

            Assert.False(result);
        }
    }

}
