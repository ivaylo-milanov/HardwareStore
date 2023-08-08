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
            this.Id = Guid.NewGuid();
        }

        [Comment("order id")]
        public Guid Id { get; set; }

        [Comment("order date")]
        [Required]
        public DateTime OrderDate { get; set; }

        [Comment("order total amount")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Comment("order status")]
        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Comment("order payment method")]
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Comment("order additional notes")]
        [MaxLength(GlobalConstants.OrderAdditionalNotesMaxLength)]
        public string? AdditionalNotes { get; set; }

        [Comment("order first name")]
        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Comment("order last name")]
        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Comment("order phone")]
        [Required]
        [MaxLength(GlobalConstants.CustomerPhoneMaxLength)]
        public string Phone { get; set; } = null!;

        [Comment("order city")]
        [Required]
        [MaxLength(GlobalConstants.CustomerCityMaxLength)]
        public string City { get; set; } = null!;

        [Comment("order area")]
        [Required]
        [MaxLength(GlobalConstants.CustomerAreaMaxLength)]
        public string Area { get; set; } = null!;

        [Comment("order address")]
        [Required]
        [MaxLength(GlobalConstants.CustomerAddressMaxLength)]
        public string Address { get; set; } = null!;

        [Comment("order customer id")]
        [Required]
        public string CustomerId { get; set; } = null!;

        [Comment("order customer")]
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public ICollection<ProductOrder> ProductsOrders { get; set; } = null!;
    }
}
