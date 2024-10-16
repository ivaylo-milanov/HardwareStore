namespace HardwareStore.Infrastructure.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("shopping cart item table")]
    public class ShoppingCartItem
    {
        [Comment("shopping cart item customer id")]
        [Required]
        public string CustomerId { get; set; } = null!;

        [Comment("shopping cart item customer")]
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

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
