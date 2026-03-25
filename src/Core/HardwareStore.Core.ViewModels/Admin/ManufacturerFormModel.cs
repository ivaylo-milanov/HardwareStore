namespace HardwareStore.Core.ViewModels.Admin
{
    using HardwareStore.Common;
    using System.ComponentModel.DataAnnotations;

    public class ManufacturerFormModel
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(GlobalConstants.ManufacturerNameMinLength)]
        [MaxLength(GlobalConstants.ManufacturerNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
