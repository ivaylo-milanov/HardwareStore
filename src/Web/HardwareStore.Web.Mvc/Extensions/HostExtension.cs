namespace HardwareStore.Extensions
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public static class HostExtension
    {
        /// <summary>
        /// Applies pending EF Core migrations (schema only). Does not insert catalog rows.
        /// To load products/categories/manufacturers, import from <c>HardwareStore.Infrastructure/Imports</c>
        /// (e.g. generate SQL from those JSON files, use SSMS import, or a one-off console tool).
        /// </summary>
        public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HardwareStoreDbContext>();
            await context.Database.MigrateAsync().ConfigureAwait(false);
            return host;
        }
    }
}
