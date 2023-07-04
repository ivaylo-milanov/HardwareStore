namespace HardwareStore.Core.ViewModels.MousePad
{
    using HardwareStore.Core.ViewModels.Product;

    public class MousePadViewModel : ProductViewModel
    {
        public string Surface { get; set; } = null!;

        public string Cover { get; set; } = null!;

        public string Backlight { get; set; } = null!;

        public string Color { get; set; } = null!; 
    }
}
