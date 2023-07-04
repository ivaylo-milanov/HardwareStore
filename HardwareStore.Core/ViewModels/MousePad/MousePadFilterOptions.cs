namespace HardwareStore.Core.ViewModels.MousePad
{
    using HardwareStore.Core.ViewModels.Product;

    public class MousePadFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Surface { get; set; } = null!;

        public IEnumerable<string> Cover { get; set; } = null!;

        public IEnumerable<string> Backlight { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;
    }
}
