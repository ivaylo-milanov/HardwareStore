namespace HardwareStore.Core.ViewModels.Processor
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.ViewModels.Product;

    [Category("Processor")]
    public class ProcessorViewModel : ProductViewModel
    {
        [Characteristic]
        public string Series { get; set; } = null!;

        [Characteristic]
        public string Generation { get; set; } = null!;

        [Characteristic]
        public string Socket { get; set; } = null!;

        [Characteristic("Box cooler")]
        public string BoxCooler { get; set; } = null!;
    }
}
