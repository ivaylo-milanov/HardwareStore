namespace HardwareStore.Extensions
{
    using HardwareStore.Infrastructure.Seed;
    using Microsoft.EntityFrameworkCore;

    public static class HostExtension
    {
        public static async Task<IHost> MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TContext>();

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var dataSeeder = services.GetRequiredService<DataSeeder>();
                        await dataSeeder.SeedData();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating or initializing the database.");

                        // Rollback transaction if any error occurs
                        transaction.Rollback();
                    }
                }
            }

            return host;
        }
    }
}
