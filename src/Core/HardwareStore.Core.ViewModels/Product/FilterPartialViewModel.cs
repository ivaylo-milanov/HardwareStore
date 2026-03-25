namespace HardwareStore.Core.ViewModels.Product
{
    public class FilterPartialViewModel
    {
        public IEnumerable<FilterCategoryModel> Filters { get; set; } = null!;

        public IReadOnlyDictionary<string, IReadOnlyList<string>> SelectedValues { get; set; } =
            new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
    }
}
