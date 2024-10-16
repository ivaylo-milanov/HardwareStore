namespace HardwareStore.Core.ViewModels.Cooler
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Cooler")]
    public class CoolerViewModel : ProductViewModel
    {
        [Characteristic]
        public string Backlight { get; set; } = null!;

        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic]
        public string Socket { get; set; } = null!;

        [Characteristic("Fan size")]
        public string FanSize { get; set; } = null!;

        [Characteristic]
        public string Connector { get; set; } = null!;
    }
}
