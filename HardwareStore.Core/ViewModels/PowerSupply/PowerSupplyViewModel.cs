namespace HardwareStore.Core.ViewModels.PowerSupply
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    public class PowerSupplyViewModel : ProductViewModel
    {
        [Characteristic]
        public int Power { get; set; }

        [Characteristic]
        public string Certificate { get; set; } = null!;

        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic("Form Factor")]
        public string FormFactor { get; set; } = null!;

        [Characteristic]
        public string Specification { get; set; } = null!;

        [Characteristic("PCIe Gen5")]
        public string PCIeGen5 { get; set; } = null!;

        [Characteristic]
        public string Backlight { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;
    }
}
