namespace HardwareStore.Extensions
{
    using Microsoft.AspNetCore.Mvc.Razor;

    public static class AddSearchPathsExtension
    {
        public static IServiceCollection AddSearchPaths(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Views/Shared/Product/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/Shared/Search/{0}.cshtml");
            });

            return services;
        }
    }
}
