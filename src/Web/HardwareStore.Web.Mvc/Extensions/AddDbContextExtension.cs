namespace HardwareStore.Extensions
{
    using HardwareStore.Common;
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public static class AddDbContextExtension
    {
        public static IServiceCollection ConfigurateDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(ExceptionMessages.DefaultConnectionNotFound);
            services.AddDbContext<HardwareStoreDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
