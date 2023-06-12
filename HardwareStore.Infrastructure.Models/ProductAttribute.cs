namespace HardwareStore.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using Common;

    [Comment("product attribute table")]
    public class ProductAttribute
    {
        [Comment("product attribute id")]
        [Key]
        public int Id { get; set; }

        [Comment("product attribute product id")]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Comment("product attribute product")]
        public virtual Product Product { get; set; } = null!;

        [Comment("product attribute key")]
        [Required]
        [MaxLength(GlobalConstants.ProductAttributeNameMaxLength)]
        //varchar
        public string Name { get; set; } = null!;

        [Comment("product attribute value")]
        [Required]
        [MaxLength(GlobalConstants.ProductAttributeValueMaxLength)]
        public string Value { get; set; } = null!;
    }
}
