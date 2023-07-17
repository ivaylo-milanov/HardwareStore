namespace HardwareStore.Extensions
{
    public static class AddSessionExtension
    {
        public static IServiceCollection AddCustomSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });

            return services;
        }
    }
}
