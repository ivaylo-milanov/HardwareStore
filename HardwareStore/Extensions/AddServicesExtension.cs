namespace HardwareStore.Extensions
{
    using Core.Services;
    using Core.Services.Contracts;
    using HardwareStore.Infrastructure.Common;

    public static class AddServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IMouseService, MouseService>();
            services.AddScoped<IKeyboardService, KeyboardService>();
            services.AddScoped<IHeadsetService, HeadsetService>();
            services.AddScoped<IFilterService, FilterService>();
            services.AddScoped<IMousePadService, MousePadService>();

            services.AddControllers().AddNewtonsoftJson();

            return services;
        }
    }
}
