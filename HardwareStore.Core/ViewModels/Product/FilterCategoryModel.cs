namespace HardwareStore.Core.ViewModels.Product
{
    public class FilterCategoryModel
    {
        public IEnumerable<string> Values { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Title { get; set; } = null!;
    }
}
