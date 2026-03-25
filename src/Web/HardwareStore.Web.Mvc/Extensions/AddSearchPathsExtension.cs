namespace HardwareStore.Extensions
{
    using Microsoft.AspNetCore.Mvc.Razor;

    public static class AddSearchPathsExtension
    {
        public static IServiceCollection AddSearchPaths(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Views/Shared/Catalog/{0}.cshtml");
            });

            return services;
        }
    }
}
