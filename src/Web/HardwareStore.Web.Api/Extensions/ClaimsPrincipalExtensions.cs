namespace HardwareStore.Web.Api.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims) =>
            claims.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
