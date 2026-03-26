namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using Common;

    [Comment("product table")]
    public class Product
    {
        public Product()
        {
            this.ProductsOrders = new HashSet<ProductOrder>();
            this.ShoppingCartItems = new HashSet<ShoppingCartItem>();
            this.Favorites = new HashSet<Favorite>();
            this.AssemblyComponents = new HashSet<ProductAssemblyComponent>();
            this.UsedInAssemblies = new HashSet<ProductAssemblyComponent>();
        }

        [Comment("product id")]
        [Key]
        public int Id { get; set; }

        [Comment("product price")]
        [Required]
        public decimal Price { get; set; }

        [Comment("product name")]
        [Required]
        [MaxLength(GlobalConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("product quantity")]
        [Required]
        public int Quantity { get; set; }

        [Comment("product description")]
        [MaxLength(GlobalConstants.ProductDescriptionMaxLength)]
        public string? Description { get; set; }

        [Comment("product add date")]
        public DateTime AddDate { get; set; }

        [Comment("product warranty")]
        [Required]
        public int Warranty { get; set; }

        [Comment("product manufacturer id")]
        public int? ManufacturerId { get; set; }

        [Comment("product manufacturer")]
        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer? Manufacturer { get; set; }

        [Comment("product model")]
        [MaxLength(GlobalConstants.ProductModelMaxLength)]
        public string? Model { get; set; }

        [Comment("product reference number")]
        [Required]
        [MaxLength(GlobalConstants.ProductReferenceNumberMaxLength)]
        //varchar
        public string ReferenceNumber { get; set; } = null!;

        [Comment("product category id")]
        [Required]
        public int CategoryId { get; set; }

        [Comment("product category")]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// JSON object: option display name → value (e.g. {"Form Factor":"Mid Tower"}).
        /// </summary>
        [Comment("product options json")]
        public string Options { get; set; } = "{}";

        public ICollection<ProductOrder> ProductsOrders { get; set; } = null!;

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;

        public ICollection<Favorite> Favorites { get; set; } = null!;

        public ICollection<ProductAssemblyComponent> AssemblyComponents { get; set; } = null!;

        public ICollection<ProductAssemblyComponent> UsedInAssemblies { get; set; } = null!;
    }
}
