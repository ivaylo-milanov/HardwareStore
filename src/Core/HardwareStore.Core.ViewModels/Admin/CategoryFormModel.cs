namespace HardwareStore.Core.ViewModels.Admin
{
    using HardwareStore.Common;
    using System.ComponentModel.DataAnnotations;

    public class CategoryFormModel
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.CategoryNameMinLength)]
        [MaxLength(GlobalConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
