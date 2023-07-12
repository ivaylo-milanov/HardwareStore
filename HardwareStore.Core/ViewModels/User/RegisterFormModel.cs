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
        [MaxLength(GlobalConstants.CustomerUserNameMaxLength)]
        [MinLength(GlobalConstants.CustomerUserNameMinLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerPasswordMaxLength)]
        [MinLength(GlobalConstants.CustomerPasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
