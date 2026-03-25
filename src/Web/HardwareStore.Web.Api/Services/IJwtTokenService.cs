namespace HardwareStore.Web.Api.Services
{
    using HardwareStore.Infrastructure.Models;

    public interface IJwtTokenService
    {
        int ExpiresInSeconds { get; }

        string CreateToken(Customer user);
    }
}
