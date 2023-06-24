namespace HardwareStore.Extensions
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.Services;

    public static class AddDropboxExtension
    {
        public static IServiceCollection AddDropboxService(this IServiceCollection services, IConfiguration configuration)
        {
            var accessKey = configuration["DropboxOptions:AccessKey"];

            services.AddScoped<IDropboxService, DropboxService>(provider =>
                new DropboxService(accessKey));

            return services;
        }
    }
}
