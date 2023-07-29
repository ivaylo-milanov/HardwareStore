namespace HardwareStore.Infrastructure.Models
{
    using HardwareStore.Common;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Comment("manufacturer table")]
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Products= new HashSet<Product>();
        }

        [Comment("manufacturer id")]
        [Key]
        public int Id { get; set; }

        [Comment("manufacturer name")]
        [Required]
        [MaxLength(GlobalConstants.ManufacturerNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = null!;
    }
}
