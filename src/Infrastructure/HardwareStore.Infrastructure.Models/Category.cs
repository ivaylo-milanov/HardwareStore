namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;

    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    using Common;

    [Comment("category table")]
    public class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Comment("category id")]
        [Key]
        public int Id { get; set; }

        [Comment("category name")]
        [Required]
        [MaxLength(GlobalConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("hardware vs peripherals segment")]
        public CategoryGroup Group { get; set; }

        [Comment("assembly BOM slot filter for products in this category; None if not used for standard slots")]
        public CategoryAssemblySlot AssemblySlot { get; set; }

        public ICollection<Product> Products { get; set; } = null!;
    }
}
