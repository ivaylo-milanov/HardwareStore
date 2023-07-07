namespace HardwareStore.Core.ViewModels.Processor
{
    using HardwareStore.Core.ViewModels.Product;

    public class ProcessorViewModel : ProductViewModel
    {
        public string Series { get; set; } = null!;

        public string Generation { get; set; } = null!;

        public string Socket { get; set; } = null!;

        public string BoxCooler { get; set; } = null!;
    }
}
