namespace HardwareStore.Web.Api.Extensions
{
    using HardwareStore.Common;
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHardwareStoreDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(ExceptionMessages.DefaultConnectionNotFound);
            services.AddDbContext<HardwareStoreDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddHardwareStoreDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
