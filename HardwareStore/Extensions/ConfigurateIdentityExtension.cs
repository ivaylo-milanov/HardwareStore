﻿namespace HardwareStore.Extensions
{
    using HardwareStore.Common;
    using HardwareStore.Infrastructure.Data;
    using HardwareStore.Infrastructure.Models;

    public static class ConfigurateIdentityExtension
    {
        public static IServiceCollection ConfigurateIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<Customer>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = GlobalConstants.CustomerPasswordMinLength;
            })
               .AddEntityFrameworkStores<HardwareStoreDbContext>();

            return services;
        }
    }
}
