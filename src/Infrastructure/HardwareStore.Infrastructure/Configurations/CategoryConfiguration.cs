namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Group).HasComment("hardware vs peripherals segment");
        }
    }
}
