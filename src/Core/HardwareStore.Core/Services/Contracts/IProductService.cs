namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;

    public interface IProductService
    {
        Task<IEnumerable<TModel>> FilterProductsAsync<TModel, TFilter>(TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel;

        Task<IEnumerable<SearchViewModel>> FilterSearchProductsAsync(string keyword, SearchFilterOptions filter);

        /// <summary>
        /// Category pages: call without <paramref name="keyword"/>. Search: use <see cref="SearchViewModel"/> and pass the keyword (null/whitespace = all products).
        /// </summary>
        Task<ProductsViewModel<TModel>> GetModel<TModel>(string? keyword = null) where TModel : ProductViewModel;
    }
}
