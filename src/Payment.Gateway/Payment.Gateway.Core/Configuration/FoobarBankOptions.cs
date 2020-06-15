namespace Payment.Gateway.Core.Configuration
{
    public class FoobarBankOptions
    {
        public string BaseUrl { get; set; }
        public string TokenPath { get; set; }
        public string PaymentsPath { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
    }
}
