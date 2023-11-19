namespace HardwareStore.Core.ViewModels.Monitor
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Monitor")]
    public class MonitorViewModel : ProductViewModel
    {
        [Characteristic]
        public string Resolution { get; set; } = null!;

        [Characteristic]
        public string Matrix { get; set; } = null!;

        [Characteristic]
        public string Ports { get; set; } = null!;

        [Characteristic]
        public string Technology { get; set; } = null!;

        [Characteristic("Stand adjustment")]
        public string PostureAdjustment { get; set; } = null!;

        [Characteristic("Touch screen")]
        public string TouchScreen { get; set; } = null!;

        [Characteristic]
        public string VESA { get; set; } = null!;

        [Characteristic]
        public string Color { get; set; } = null!; 
    }
}
