namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductFilterOptions
    {
        public int Order { get; set; }

        public IEnumerable<string> Manufacturer { get; set; } = null!;
    }
}
