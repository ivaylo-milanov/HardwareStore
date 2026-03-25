namespace HardwareStore.Core.ViewModels.Admin
{
    using HardwareStore.Common;
    using System.ComponentModel.DataAnnotations;

    public class ProductFormModel
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.ProductNameMinLength)]
        [MaxLength(GlobalConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999.99")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [MaxLength(GlobalConstants.ProductDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Warranty { get; set; }

        public int? ManufacturerId { get; set; }

        [MaxLength(GlobalConstants.ProductModelMaxLength)]
        public string? Model { get; set; }

        [Required]
        [MinLength(GlobalConstants.ProductReferenceNumberMinLength)]
        [MaxLength(GlobalConstants.ProductReferenceNumberMaxLength)]
        public string ReferenceNumber { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public string Options { get; set; } = "{}";

        public DateTime? AddDate { get; set; }
    }
}
