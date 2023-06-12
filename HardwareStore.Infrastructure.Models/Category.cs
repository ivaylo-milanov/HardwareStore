namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<Product> Products { get; set; } = null!;
    }
}
