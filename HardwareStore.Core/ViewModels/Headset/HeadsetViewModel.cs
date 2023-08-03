namespace HardwareStore.Core.ViewModels.Headset
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Headset")]
    public class HeadsetViewModel : ProductViewModel
    {
        [Characteristic]
        public string Form { get; set; } = null!;

        [Characteristic]
        public string Interface { get; set; } = null!;

        [Characteristic]
        public string Soundproofing { get; set; } = null!;

        [Characteristic]
        public string Type { get; set; } = null!;

        [Characteristic]
        public string Compatibility { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!;
    }
}
