namespace HardwareStore.Extensions
{
    using Core.Services;
    using Core.Services.Contracts;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Seed;
    using HardwareStore.Infrastructure.Seed.Contracts;
    using Newtonsoft.Json;

    public static class AddServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IFileReader, FileReader>();
            services.AddScoped<IDataSeeder, DataSeeder>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDetailsService, DetailsService>();

            services.AddControllers();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
