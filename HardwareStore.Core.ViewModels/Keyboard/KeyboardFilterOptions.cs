namespace HardwareStore.Core.ViewModels.Keyboard
{
    using HardwareStore.Core.ViewModels.Product;

    public class KeyboardFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Connectivity { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;

        public IEnumerable<string> Type { get; set; } = null!;

        public IEnumerable<string> Form { get; set; } = null!;

        public IEnumerable<string> Backlight { get; set; } = null!;

        public IEnumerable<string> Cyrillicization { get; set; } = null!;

        public IEnumerable<string> ButtonType { get; set; } = null!;

        public IEnumerable<string> MacroButtons { get; set; } = null!;

        public IEnumerable<string> MultiMediaButtons { get; set; } = null!;

        public IEnumerable<string> Switch { get; set; } = null!;

        public IEnumerable<string> Layout { get; set; } = null!;

        public IEnumerable<string> HotSwappable { get; set; } = null!;
    }
}
