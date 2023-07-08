namespace HardwareStore.Core.ViewModels.Ram
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    public class RamViewModel : ProductViewModel
    {
        [Characteristic]
        public string Type { get; set; } = null!;

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
