namespace HardwareStore.Core.ViewModels.Headset
{
    using HardwareStore.Core.ViewModels.Product;

    public class HeadsetViewModel : ProductViewModel
    {
        public string Form { get; set; } = null!;

        public string Interface { get; set; } = null!;

        public string NoiseIsolation { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Compatibility { get; set; } = null!;

        public string Color { get; set; } = null!;
    }
}
