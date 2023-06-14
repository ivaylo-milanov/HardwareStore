namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ComputerPartConfiguration : IEntityTypeConfiguration<ComputerPart>
    {
        public void Configure(EntityTypeBuilder<ComputerPart> builder)
        {
            builder.HasKey(e => new { e.PartId, e.ComputerId });

            builder
                .HasOne(cp => cp.Part)
                .WithMany(p => p.PartComputers)
                .HasForeignKey(cp => cp.PartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(cp => cp.Computer)
                .WithMany(p => p.ComputerParts)
                .HasForeignKey(cp => cp.ComputerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
