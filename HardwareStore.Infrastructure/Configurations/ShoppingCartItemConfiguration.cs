﻿namespace HardwareStore.Infrastructure.Configurations
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.ToTable("ShoppingCartItems");
            builder
                .HasKey(sci => new { sci.ProductId, sci.UserId });

            builder
                .HasOne(sci => sci.User)
                .WithMany(c => c.ShoppingCartItems)
                .HasForeignKey(sci => sci.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
