namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CharacteristicConfiguraion : IEntityTypeConfiguration<Characteristic>
    {
        public void Configure(EntityTypeBuilder<Characteristic> builder)
        {
            builder.ToTable("Characteristics");

            builder
                .HasKey(c => new { c.ProductId, c.CharacteristicNameId });

            builder
                .HasOne(f => f.Product)
                .WithMany(p => p.Characteristics)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(f => f.CharacteristicName)
                .WithMany(p => p.Characteristics)
                .HasForeignKey(f => f.CharacteristicNameId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
