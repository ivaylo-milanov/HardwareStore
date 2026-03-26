namespace HardwareStore.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// EF Core CLI design-time: connection string from <c>ConnectionStrings__DefaultConnection</c> (environment/CI)
    /// or User Secrets on the MVC/API web projects — not from appsettings.
    /// </summary>
    public sealed class HardwareStoreDbContextFactory : IDesignTimeDbContextFactory<HardwareStoreDbContext>
    {
        private const string MvcUserSecretsId = "aspnet-HardwareStore.Web.Mvc-4ee43552-cae6-4ee5-9b2b-edd6b1dc975f";
        private const string ApiUserSecretsId = "aspnet-HardwareStore.Web.Api-8c2e9f1a-4b3d-4e5f-9a0b-1c2d3e4f5a6b";

        public HardwareStoreDbContext CreateDbContext(string[] args)
        {
            var connectionString = ResolveConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Design-time connection string not found. Use User Secrets: " +
                    "dotnet user-secrets set \"ConnectionStrings:DefaultConnection\" \"<your-connection>\" " +
                    "--project src/Web/HardwareStore.Web.Mvc (or .Web.Api), " +
                    "or set environment variable ConnectionStrings__DefaultConnection.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<HardwareStoreDbContext>();
            optionsBuilder.UseHardwareStoreSqlServer(connectionString);
            return new HardwareStoreDbContext(optionsBuilder.Options);
        }

        private static string? ResolveConnectionString()
        {
            var fromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            if (!string.IsNullOrWhiteSpace(fromEnv))
            {
                return fromEnv;
            }

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            foreach (var (basePath, userSecretsId) in GetCandidateWebProjectRoots())
            {
                if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
                {
                    continue;
                }

                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddUserSecrets(userSecretsId)
                    .AddEnvironmentVariables()
                    .Build();

                var cs = config.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(cs))
                {
                    return cs;
                }
            }

            return null;
        }

        private static IEnumerable<(string BasePath, string UserSecretsId)> GetCandidateWebProjectRoots()
        {
            var result = new List<(string, string)>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void Add(string path, string userSecretsId)
            {
                var full = Path.GetFullPath(path);
                if (!Directory.Exists(full) || !seen.Add(full))
                {
                    return;
                }

                result.Add((full, userSecretsId));
            }

            var cwd = Directory.GetCurrentDirectory();
            var cwdName = new DirectoryInfo(cwd).Name;
            if (cwdName.Equals("HardwareStore.Web.Api", StringComparison.OrdinalIgnoreCase))
            {
                Add(cwd, ApiUserSecretsId);
            }
            else if (cwdName.Equals("HardwareStore.Web.Mvc", StringComparison.OrdinalIgnoreCase))
            {
                Add(cwd, MvcUserSecretsId);
            }
            else
            {
                Add(cwd, MvcUserSecretsId);
            }

            var dir = new DirectoryInfo(cwd);
            for (var i = 0; i < 10 && dir != null; i++, dir = dir.Parent!)
            {
                Add(Path.Combine(dir.FullName, "src", "Web", "HardwareStore.Web.Mvc"), MvcUserSecretsId);
                Add(Path.Combine(dir.FullName, "src", "Web", "HardwareStore.Web.Api"), ApiUserSecretsId);
            }

            return result;
        }
    }
}
