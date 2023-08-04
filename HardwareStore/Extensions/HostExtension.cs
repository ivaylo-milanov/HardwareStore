namespace HardwareStore.Extensions
{
    using HardwareStore.Infrastructure.Data;
    using HardwareStore.Infrastructure.Seed.Contracts;

    public static class HostExtension
    {
        public static async Task<IHost> MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<HardwareStoreDbContext>();
                var dataSeeder = services.GetRequiredService<IDataSeeder>();

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await dataSeeder.SeedCharacteristicsNames(context);
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await dataSeeder.SeedManufacturers(context);
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await dataSeeder.SeedCategories(context);
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await dataSeeder.SeedProducts(context);
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
