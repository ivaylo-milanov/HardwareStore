namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    [Comment("product order table")]
    public class ProductOrder
    {
        [Comment("product order product id")]
        [Required]
        public int ProductId { get; set; }

        [Comment("product order product")]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Comment("product order order id")]
        [Required]
        public Guid OrderId { get; set; }

        [Comment("product order product")]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [Comment("product order quantity")]
        public int Quantity { get; set; }
    }
}
