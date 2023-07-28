namespace HardwareStore.Infrastructure.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("shopping cart item table")]
    public class ShoppingCartItem
    {
        [Comment("shopping cart item user id")]
        [Required]
        public string UserId { get; set; } = null!;

        [Comment("shopping cart item user")]
        [ForeignKey(nameof(UserId))]
        public Customer User { get; set; } = null!;

        [Comment("shopping cart item product id")]
        [Required]
        public int ProductId { get; set; }

        [Comment("shopping cart item product")]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Comment("shopping cart item quantity")]
        [Required]
        public int Quantity { get; set; }
    }
}
