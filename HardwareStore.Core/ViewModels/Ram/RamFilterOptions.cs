namespace HardwareStore.Core.ViewModels.Ram
{
    using HardwareStore.Core.ViewModels.Product;

    public class RamFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> Heatsink { get; set; } = null!;

        public IEnumerable<string> Kit { get; set; } = null!;

        public IEnumerable<string> Backlight { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;
    }
}
