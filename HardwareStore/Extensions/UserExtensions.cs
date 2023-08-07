namespace HardwareStore.Extensions
{
    using System.Security.Claims;

    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims) => claims.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
