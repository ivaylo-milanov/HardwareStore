namespace HardwareStore.Core.ViewModels.Keyboard
{
    using HardwareStore.Core.ViewModels.Product;

    public class KeyboardViewModel : ProductViewModel
    {
        public string Connectivity { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string Interface { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Form { get; set; } = null!;

        public string Backlight { get; set; } = null!;

        public string Cyrillicization { get; set; } = null!;

        public string ButtonType { get; set; } = null!;

        public string MacroButtons { get; set; } = null!;

        public string MultiMediaButtons { get; set; } = null!;

        public string Switch { get; set; } = null!;

        public string Layout { get; set; } = null!;

        public string HotSwappable { get; set; } = null!; 
    }
}
