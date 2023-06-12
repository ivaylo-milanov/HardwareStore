namespace HardwareStore.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Infrastructure.Configurations;

    public class HardwareStoreDbContext : IdentityDbContext<Customer>
    {
        public HardwareStoreDbContext(DbContextOptions<HardwareStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<ProductAttribute> ProductsAttributes { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<ProductOrder> ProductsOrders { get; set; } = null!;

        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        public DbSet<ComputerPart> ComputerParts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductOrderConfiguration());
            builder.ApplyConfiguration(new ComputerPartConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            
            base.OnModelCreating(builder);
        }

    }
}