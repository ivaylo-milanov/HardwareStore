namespace HardwareStore.Core.ViewModels.User
{
    using HardwareStore.Common;
    using System.ComponentModel.DataAnnotations;

    public class RegisterFormModel
    {
        [Required]
        [MaxLength(GlobalConstants.CustomerEmailMaxLength)]
        [MinLength(GlobalConstants.CustomerEmailMinLength)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerPasswordMaxLength)]
        [MinLength(GlobalConstants.CustomerPasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerPhoneMaxLength)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerCityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAreaMaxLength)]
        public string Area { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerAddressMaxLength)]
        public string Address { get; set; } = null!;
    }
}
