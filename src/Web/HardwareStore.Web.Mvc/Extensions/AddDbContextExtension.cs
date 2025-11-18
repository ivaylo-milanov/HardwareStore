namespace HardwareStore.Extensions
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public static class AddDbContextExtension
    {
        public static IServiceCollection ConfigurateDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<HardwareStoreDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
