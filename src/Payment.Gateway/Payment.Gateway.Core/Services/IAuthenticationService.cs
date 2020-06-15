namespace Payment.Gateway.Core.Services
{
    public interface IAuthenticationService
    {
        bool ValidateToken(string token);
    }
}