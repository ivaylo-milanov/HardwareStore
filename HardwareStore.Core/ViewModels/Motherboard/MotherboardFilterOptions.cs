namespace HardwareStore.Core.ViewModels.Motherboard
{
    using HardwareStore.Core.ViewModels.Product;

    public class MotherboardFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Socket { get; set; } = null!;

        public IEnumerable<string> FormFactor { get; set; } = null!;

        public IEnumerable<string> Chipset { get; set; } = null!;

        public IEnumerable<string> MaintenanceMemory { get; set; } = null!;

        public IEnumerable<string> BuiltInWIFI { get; set; } = null!;

        public IEnumerable<string> Backlight { get; set; } = null!;
    }
}
