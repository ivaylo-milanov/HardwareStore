namespace HardwareStore.Core.ViewModels.Processor
{
    using HardwareStore.Core.ViewModels.Product;

    public class ProcessorFilterOptions : ProductFilterOptions
    {
        public IEnumerable<string> Series { get; set; } = null!;

        public IEnumerable<string> Generation { get; set; } = null!;

        public IEnumerable<string> Socket { get; set; } = null!;

        public IEnumerable<string> BoxCooler { get; set; } = null!;
    }
}
