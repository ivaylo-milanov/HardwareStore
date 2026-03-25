namespace HardwareStore.Core.ViewModels.Admin
{
    using HardwareStore.Common;
    using System.ComponentModel.DataAnnotations;

    public class CustomerEditFormModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerCityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAreaMaxLength)]
        public string Area { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAddressMaxLength)]
        public string Address { get; set; } = null!;

        [Display(Name = "Administrator")]
        public bool IsAdmin { get; set; }
    }
}
