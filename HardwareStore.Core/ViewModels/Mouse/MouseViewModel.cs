namespace HardwareStore.Core.ViewModels.Mouse
{
    using HardwareStore.Core.ViewModels.Product;

    public class MouseViewModel : ProductViewModel
    {
        public string Connectivity { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string Interface { get; set; } = null!;

        public string Sensor { get; set; } = null!;

        public int NumberOfKeys { get; set; }
    }
}
