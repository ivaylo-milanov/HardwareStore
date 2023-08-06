namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;

    public interface IProductService
    {
        IEnumerable<TModel> FilterProducts<TModel, TFilter>(TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel;

        Task<ProductsViewModel<SearchViewModel>> GetSearchModel(string keyword);

        Task<ProductDetailsModel> GetProductDetails(int productId);

        Task<ProductsViewModel<TModel>> GetModel<TModel>() where TModel : ProductViewModel;
    }
}
