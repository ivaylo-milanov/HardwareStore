namespace HardwareStore.Core.ViewModels.Monitor
{
    using HardwareStore.Core.ViewModels.Product;

    public class MonitorFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Resolution { get; set; } = null!;

        public IEnumerable<string> Matrix { get; set; } = null!;

        public IEnumerable<string> Ports { get; set; } = null!;

        public IEnumerable<string> Technology { get; set; } = null!;

        public IEnumerable<string> StandAdjustment { get; set; } = null!;

        public IEnumerable<string> TouchScreen { get; set; } = null!;

        public IEnumerable<string> VESA { get; set; } = null!;

        public IEnumerable<string> Color { get; set; } = null!;
    }
}
