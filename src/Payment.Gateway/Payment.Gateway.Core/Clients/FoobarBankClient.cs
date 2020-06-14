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

    public class FoobarBankClient : IFoobarBankClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FoobarBankOptions _foobarBankOptions;
        private readonly string _tokenRequestAddress;
        private readonly string _paymentRequestAddress;
        private readonly ILogger _logger;
        private const string ApplicationJson = "application/json";
        private const string SuccessfulPaymentStatus = "success";
        private const string DeclinedPaymentStatus = "declined";

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
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception deserializing token response from {_tokenRequestAddress}");
                    return new AuthenticationResult()
                    {
                        IsSuccessful = false,
                        Token = string.Empty
                    };
                }
            }


            _logger.LogError($"No successful login to foobar bank {_tokenRequestAddress}");

            return new AuthenticationResult()
            {
                IsSuccessful = false,
                Token = string.Empty
            };
        }


        public async Task<PaymentResult> CreatePayment(CardPayment cardPayment)
        {

            var authToken = await GetAuthenticationToken();
            if (!authToken.IsSuccessful)
            {
                return new PaymentResult()
                {
                    PaymentStatus = PaymentStatus.Error,
                    Reference = cardPayment.Reference
                };
            
            }

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

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                try
                {
                    var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(rawContent);

                    if (string.Equals(paymentResponse.PaymentStatus, SuccessfulPaymentStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _logger.LogInformation($"Successfully created payment for Reference={cardPayment.Reference} ThirdPartyReference={paymentResponse.FoobarReference}");
                        return new PaymentResult()
                        {
                            PaymentStatus = PaymentStatus.Success,
                            Reference = cardPayment.Reference,
                            ThirdPartyReference = paymentResponse.FoobarReference
                        };
                    }
                    else if (string.Equals(paymentResponse.PaymentStatus, DeclinedPaymentStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _logger.LogInformation($"Payment declined for Reference={cardPayment.Reference} ThirdPartyReference={paymentResponse.FoobarReference}");
                        return new PaymentResult()
                        {
                            PaymentStatus = PaymentStatus.Declined,
                            Reference = cardPayment.Reference,
                            ThirdPartyReference = paymentResponse.FoobarReference
                        };
                    }
                    else
                    {
                        _logger.LogError($"Unknown payment status response \"{paymentResponse.PaymentStatus}\"from {_paymentRequestAddress} for reference={cardPayment.Reference}");
                        return new PaymentResult()
                        {
                            PaymentStatus = PaymentStatus.Error,
                            Reference = cardPayment.Reference
                        };
                    }
                }
                catch
                {
                    _logger.LogError($"Error deserializing payment response from {_paymentRequestAddress} for reference={cardPayment.Reference}");
                    return new PaymentResult()
                    {
                        PaymentStatus = PaymentStatus.Error,
                        Reference = cardPayment.Reference
                    };
                }
            }

            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError($"Error requesting payment from {_paymentRequestAddress} for reference={cardPayment.Reference}");

                return new PaymentResult()
                {
                    PaymentStatus = PaymentStatus.Error,
                    Reference = cardPayment.Reference
                };
            }

            _logger.LogError($"Unknown http response code from {_paymentRequestAddress} for reference={cardPayment.Reference}");

            return new PaymentResult()
            {
                PaymentStatus = PaymentStatus.Error,
                Reference = cardPayment.Reference
            };
        }
    }
}
