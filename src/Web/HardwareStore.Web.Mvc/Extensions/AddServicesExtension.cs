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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
