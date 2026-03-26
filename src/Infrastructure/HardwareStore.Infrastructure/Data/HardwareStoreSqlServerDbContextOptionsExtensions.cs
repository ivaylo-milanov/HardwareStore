namespace HardwareStore.Infrastructure.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Shared SQL Server options. Retries when the engine is still in script-upgrade mode (error 18401),
    /// which happens briefly after a Docker container starts or after certain upgrades.
    /// </summary>
    public static class HardwareStoreSqlServerDbContextOptionsExtensions
    {
        private static readonly int[] TransientErrorNumbersIncludingScriptUpgrade = { 18401 };

        public static DbContextOptionsBuilder UseHardwareStoreSqlServer(
            this DbContextOptionsBuilder builder,
            string connectionString)
        {
            return builder.UseSqlServer(connectionString, sql =>
                sql.EnableRetryOnFailure(
                    maxRetryCount: 12,
                    maxRetryDelay: TimeSpan.FromSeconds(15),
                    errorNumbersToAdd: TransientErrorNumbersIncludingScriptUpgrade));
        }
    }
}
