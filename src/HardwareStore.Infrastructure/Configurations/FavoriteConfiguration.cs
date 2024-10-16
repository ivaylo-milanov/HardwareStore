namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");
            builder
                .HasKey(f => new { f.ProductId, f.CustomerId });

            builder
                .HasOne(f => f.Customer)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
