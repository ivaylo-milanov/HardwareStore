namespace HardwareStore.Core.ViewModels.Product
{
    public class ProductsViewModel<TModel> where TModel : ProductViewModel
    {
        public IEnumerable<TModel> Products { get; set; } = null!;

        public IEnumerable<FilterCategoryModel> Filters { get; set; } = null!;
    }
}
