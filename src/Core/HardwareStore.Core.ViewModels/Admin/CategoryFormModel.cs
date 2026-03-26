namespace HardwareStore.Core.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;
    using HardwareStore.Common;
    using HardwareStore.Infrastructure.Models.Enums;

    public class CategoryFormModel
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.CategoryNameMinLength)]
        [MaxLength(GlobalConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Segment")]
        public CategoryGroup Group { get; set; }
    }
}
