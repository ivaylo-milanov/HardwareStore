namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductAssemblyComponentConfiguration : IEntityTypeConfiguration<ProductAssemblyComponent>
    {
        public void Configure(EntityTypeBuilder<ProductAssemblyComponent> builder)
        {
            builder.HasIndex(e => e.AssemblyProductId);

            builder
                .HasOne(e => e.AssemblyProduct)
                .WithMany(p => p.AssemblyComponents)
                .HasForeignKey(e => e.AssemblyProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(e => e.ComponentProduct)
                .WithMany(p => p.UsedInAssemblies)
                .HasForeignKey(e => e.ComponentProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
