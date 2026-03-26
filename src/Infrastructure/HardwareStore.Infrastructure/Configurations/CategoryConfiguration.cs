namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Group).HasComment("hardware vs peripherals segment");
            builder
                .Property(c => c.AssemblySlot)
                .HasDefaultValue(CategoryAssemblySlot.None)
                .HasComment("assembly BOM slot filter for products in this category; None if not used for standard slots");
        }
    }
}
