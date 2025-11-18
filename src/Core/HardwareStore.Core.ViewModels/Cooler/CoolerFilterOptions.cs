namespace HardwareStore.Core.ViewModels.Cooler
{
    using HardwareStore.Core.ViewModels.Product;

    public class CoolerFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Backlight { get; set; } = null!;

        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> Socket { get; set; } = null!;

        public IEnumerable<string> FanSize { get; set; } = null!;

        public IEnumerable<string> Connector { get; set; } = null!;
    }
}
