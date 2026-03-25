namespace HardwareStore.Tests.Integration
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;

    public class HardwareStoreWebApplicationFactory : WebApplicationFactory<Program>
    {
        #region Configuration

        private const string DummySqlConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=IntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true";

        private readonly string? environmentName;
        private readonly string inMemoryDatabaseName = "HardwareStoreTests_" + Guid.NewGuid().ToString("N");

        public HardwareStoreWebApplicationFactory(string? environmentName = null)
        {
            this.environmentName = environmentName;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.Add(
                    new MemoryConfigurationSource
                    {
                        InitialData = new Dictionary<string, string?>
                        {
                            ["ConnectionStrings:DefaultConnection"] = DummySqlConnectionString,
                        },
                    });
            });

            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (!string.IsNullOrEmpty(this.environmentName))
            {
                builder.UseEnvironment(this.environmentName);
            }

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<HardwareStoreDbContext>>();
                services.RemoveAll<HardwareStoreDbContext>();

                services.AddDbContext<HardwareStoreDbContext>(options =>
                    options.UseInMemoryDatabase(this.inMemoryDatabaseName));
            });
        }

        #endregion
    }
}
