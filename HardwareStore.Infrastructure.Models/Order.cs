namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using Common;
    using Enums;

    [Comment("order table")]
    public class Order
    {
        public Order()
        {
            this.ProductsOrders = new HashSet<ProductOrder>();
        }

        [Comment("order id")]
        public int Id { get; set; }

        [Comment("order date")]
        [Required]
        public DateTime OrderDate { get; set; }

        [Comment("order total amount")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Comment("order status")]
        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Comment("order billing address")]
        [Required]
        [MaxLength(GlobalConstants.OrderBillingAddressMaxLength)]
        public string BillingAddress { get; set; } = null!;

        [Comment("order shipping address")]
        [Required]
        [MaxLength(GlobalConstants.OrderBillingAddressMaxLength)]
        public string ShippingAddress { get; set; } = null!;

        [Comment("order payment method")]
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Comment("order additional notes")]
        public string? AdditionalNotes { get; set; }

        [Comment("order customer id")]
        [Required]
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; } = null!;

        [Comment("order customer")]
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<ProductOrder> ProductsOrders { get; set; } = null!;
    }
}
