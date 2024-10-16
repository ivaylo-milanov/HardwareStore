namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            builder.ToTable("ProductsOrders");
            builder.HasKey(e => new { e.OrderId, e.ProductId });

            builder
                .HasOne(po => po.Order)
                .WithMany(p => p.ProductsOrders)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(po => po.Product)
                .WithMany(p => p.ProductsOrders)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
