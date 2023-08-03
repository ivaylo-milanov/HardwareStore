using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Infrastructure.Data;
namespace HardwareStore
{
    using HardwareStore.Extensions;
    using HardwareStore.Infrastructure.Seed;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.ConfigurateDbContext(builder.Configuration);
            builder.Services.AddDropboxService(builder.Configuration);
            builder.Services.ConfigurateIdentity();
            builder.Services.AddServices();
            builder.Services.AddSearchPaths();
            builder.Services.AddCustomSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dataSeeder = services.GetRequiredService<DataSeeder>();
                    await dataSeeder.SeedData();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }

            await app.RunAsync();
        }
    }
}