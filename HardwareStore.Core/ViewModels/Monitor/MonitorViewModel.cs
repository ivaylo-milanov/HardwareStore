namespace HardwareStore.Core.ViewModels.Monitor
{
    using HardwareStore.Core.ViewModels.Product;

    public class MonitorViewModel : ProductViewModel
    {
        public string Resolution { get; set; } = null!;

        public string Matrix { get; set; } = null!;

        public string Ports { get; set; } = null!;

        public string Technology { get; set; } = null!;

        public string StandAdjustment { get; set; } = null!;

        public string TouchScreen { get; set; } = null!;

        public string VESA { get; set; } = null!;

        public string Color { get; set; } = null!; 
    }
}
