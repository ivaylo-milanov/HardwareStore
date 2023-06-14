﻿namespace HardwareStore.Infrastructure.Models
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
            this.ComputerParts = new HashSet<ComputerPart>();
            this.ProductAttributes = new HashSet<ProductAttribute>();
            this.ProductsOrders = new HashSet<ProductOrder>();
            this.ComputerParts = new HashSet<ComputerPart>();
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
        [ForeignKey(nameof(Manufacturer))]
        public int? ManufacturerId { get; set; }

        [Comment("product manufacturer")]
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
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Comment("product category")]
        public Category Category { get; set; } = null!;

        [InverseProperty(nameof(ComputerPart.Part))]
        public virtual ICollection<ComputerPart> PartComputers { get; set; }

        [InverseProperty(nameof(ComputerPart.Computer))]
        public virtual ICollection<ComputerPart> ComputerParts { get; set; }

        public ICollection<ProductAttribute> ProductAttributes { get; set; }

        public ICollection<ProductOrder> ProductsOrders { get; set; } = null!;
    }
}
