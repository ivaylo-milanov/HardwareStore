namespace HardwareStore.Core.ViewModels.Order
{
    using HardwareStore.Common;
    using HardwareStore.Infrastructure.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class OrderFormModel
    {
        [Required]
        [MaxLength(GlobalConstants.CustomerFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.CustomerLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
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

        [Required]
        public decimal TotalAmount { get; set; }

        [MaxLength(GlobalConstants.OrderAdditionalNotesMaxLength)]
        public string? AdditionalNotes { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
