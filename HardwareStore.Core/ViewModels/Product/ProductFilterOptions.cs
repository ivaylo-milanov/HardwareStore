namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductFilterOptions
    {
        public string Order { get; set; } = null!;

        public List<string> Manufacturer { get; set; } = new List<string>();
    }
}
