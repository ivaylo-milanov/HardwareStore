namespace HardwareStore.Web.Api.Extensions
{
    using HardwareStore.Common;
    using Microsoft.AspNetCore.Identity;

    public static class CustomerIdentityOptions
    {
        public static void Apply(IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = GlobalConstants.CustomerPasswordMinLength;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        }
    }
}
