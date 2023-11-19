namespace HardwareStore.Core.ViewModels.Ram
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("RAM")]
    public class RamViewModel : ProductViewModel
    {
        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic]
        public string Purpose { get; set; } = null!;

        [Characteristic]
        public string Heatsink { get; set; } = null!;

        [Characteristic]
        public string Kit { get; set; } = null!;

        [Characteristic]
        public string Backlight { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;
    }
}
