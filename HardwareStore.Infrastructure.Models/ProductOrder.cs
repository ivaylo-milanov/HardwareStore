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
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Comment("product order product")]
        public virtual Product Product { get; set; } = null!;

        [Comment("product order order id")]
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [Comment("product order product")]
        public virtual Order Order { get; set; } = null!;

        [Comment("product order quantity")]
        public int Quantity { get; set; }
    }
}
