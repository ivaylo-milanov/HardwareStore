using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Infrastructure.Data;
namespace HardwareStore
{
    using HardwareStore.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews();

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

            app.Run();
        }
    }
}