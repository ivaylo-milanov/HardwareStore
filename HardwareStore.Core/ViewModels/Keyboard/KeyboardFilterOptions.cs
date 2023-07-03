namespace HardwareStore.Core.ViewModels.Keyboard
{
    using HardwareStore.Core.ViewModels.Product;

    public class KeyboardFilterOptions : ProductFilterOptions
    {
        public List<string> Connectivity { get; set; } = new List<string>();

        public List<string> Color { get; set; } = new List<string>();

        public List<string> Interface { get; set; } = new List<string>();

        public List<string> Type { get; set; } = new List<string>();

        public List<string> Form { get; set; } = new List<string>();

        public List<string> Backlight { get; set; } = new List<string>();

        public List<string> Cyrillicization { get; set; } = new List<string>();

        public List<string> ButtonType { get; set; } = new List<string>();

        public List<string> MacroButtons { get; set; } = new List<string>();

        public List<string> MultiMediaButtons { get; set; } = new List<string>();

        public List<string> Switch { get; set; } = new List<string>();

        public List<string> Layout { get; set; } = new List<string>();

        public List<string> HotSwappable { get; set; } = new List<string>();
    }
}
