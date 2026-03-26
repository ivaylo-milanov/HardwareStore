namespace HardwareStore.Infrastructure.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum CategoryGroup
    {
        [Display(Name = "Hardware")]
        Hardware = 0,

        [Display(Name = "Peripherals")]
        Peripherals = 1,
    }
}
