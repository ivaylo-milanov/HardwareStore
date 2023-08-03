namespace HardwareStore.Core.ViewModels.Keyboard
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Keyboard")]
    public class KeyboardViewModel : ProductViewModel
    {
        [Characteristic]
        public string Connectivity { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;

        [Characteristic]
        public string Interface { get; set; } = null!;

        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic]
        public string Form { get; set; } = null!;

        [Characteristic]
        public string Backlight { get; set; } = null!;

        [Characteristic]
        public string Cyrillicization { get; set; } = null!;

        [Characteristic("Type of buttons")]
        public string ButtonType { get; set; } = null!;

        [Characteristic("Macro buttons")]
        public string MacroButtons { get; set; } = null!;

        [Characteristic("Multi media buttons")]
        public string MultiMediaButtons { get; set; } = null!;

        [Characteristic]
        public string Switch { get; set; } = null!;

        [Characteristic]
        public string Layout { get; set; } = null!;

        [Characteristic("Hot-swappable")]
        public string HotSwappable { get; set; } = null!; 
    }
}
