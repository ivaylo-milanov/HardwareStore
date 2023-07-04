namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductFilterOptions
    {
        public string Order { get; set; } = null!;

        public IEnumerable<string> Manufacturer { get; set; } = null!;
    }
}
