namespace HardwareStore.Core.ViewModels.MousePad
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Mouse Pad")]
    public class MousePadViewModel : ProductViewModel
    {
        [Characteristic]
        public string Surface { get; set; } = null!;

        [Characteristic]
        public string Cover { get; set; } = null!;

        [Characteristic]
        public string Backlight { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!; 
    }
}
