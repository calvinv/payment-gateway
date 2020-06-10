using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Payment.Gateway.Core.Configuration;
using Newtonsoft.Json;
using Payment.Gateway.Core.Models.FoobarBank;
using System.Net;
using Payment.Gateway.Core.Models;
using System.Dynamic;

namespace Payment.Gateway.Core.Clients
{

    public class FoobarBankClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FoobarBankOptions _foobarBankOptions;
        private readonly string _tokenRequestAddress;
        private readonly string _paymentRequestAddress;
        private readonly ILogger _logger;
        private const string ApplicationJson = "application/json";

        public FoobarBankClient(IHttpClientFactory httpClientFactory, ILogger logger, IOptions<FoobarBankOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _foobarBankOptions = options.Value;
            _tokenRequestAddress = $"{_foobarBankOptions.BaseUrl}{_foobarBankOptions.TokenPath}";
            _paymentRequestAddress = $"{_foobarBankOptions.BaseUrl}{_foobarBankOptions.PaymentsPath}";

            _logger = logger;
        }

        public async Task<AuthenticationResult> GetAuthenticationToken()
        {
            var json = JsonConvert.SerializeObject(new TokenRequest()
            {
                ClientId = _foobarBankOptions.ClientId,
                ClientSecret = _foobarBankOptions.ClientSecret
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, _tokenRequestAddress)
            {
                Content = new StringContent(json, Encoding.UTF8, ApplicationJson)
            };

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            var rawContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var authToken = JsonConvert.DeserializeObject<TokenResponse>(rawContent);
                    return new AuthenticationResult()
                    {
                        IsSuccessful = true,
                        Token = authToken.Token
                    };
                }
                catch
                {
                    _logger.LogError($"Error deserializing token response from {_tokenRequestAddress}");
                    return new AuthenticationResult()
                    {
                        IsSuccessful = false,
                        Token = string.Empty
                    };
                }
            }

            return new AuthenticationResult()
            {
                IsSuccessful = false,
                Token = string.Empty
            };
        }


        public async Task<PaymentResult> CreatePayment(CardPayment cardPayment)
        {
            var json = JsonConvert.SerializeObject(new PaymentRequest()
            {
                Amount = cardPayment.PaymentAmount.Amount,
                Currency = cardPayment.PaymentAmount.Currency,
                CardNumber = cardPayment.CardDetails.CardNumber,
                Cvv = cardPayment.CardDetails.CardNumber,
                ExpiryMonth = cardPayment.CardDetails.ExpiryMonth,
                ExpiryYear = cardPayment.CardDetails.ExpiryYear,
                NameOnCard = cardPayment.CustomerDetails.CustomerName,
                Reference = cardPayment.Reference                
            });

            using var request = new HttpRequestMessage(HttpMethod.Post, _paymentRequestAddress)
            {
                Content = new StringContent(json, Encoding.UTF8, ApplicationJson)
            };

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            var rawContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var authToken = JsonConvert.DeserializeObject<TokenResponse>(rawContent);
                    return new AuthenticationResult()
                    {
                        IsSuccessful = true,
                        Token = authToken.Token
                    };
                }
                catch
                {
                    _logger.LogError($"Error deserializing token response from {_paymentRequestAddress}");
                    return new AuthenticationResult()
                    {
                        IsSuccessful = false,
                        Token = string.Empty
                    };
                }
            }

            return new AuthenticationResult()
            {
                IsSuccessful = false,
                Token = string.Empty
            };
        }


        // pay
    }
}
