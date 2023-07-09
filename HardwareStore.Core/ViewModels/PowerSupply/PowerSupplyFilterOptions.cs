namespace HardwareStore.Core.ViewModels.PowerSupply
{
    using HardwareStore.Core.ViewModels.Product;

    public class PowerSupplyFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Power { get; set; } = null!;

        public IEnumerable<string> Certificate { get; set; } = null!;

        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> FormFactor { get; set; } = null!;

        public IEnumerable<string> Specification { get; set; } = null!;

        public IEnumerable<string> PCIeGen5 { get; set; } = null!;

        public IEnumerable<string> Backlight { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;
    }
}
