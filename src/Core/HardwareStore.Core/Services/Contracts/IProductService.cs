namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Details;
    using HardwareStore.Core.ViewModels.Home;
    using HardwareStore.Core.ViewModels.Product;

    public interface IProductService
    {
        Task<HomeViewModel> GetHomeViewModelAsync();

        Task<ProductsViewModel<CatalogProductViewModel>> GetCategoryCatalogAsync(string categoryName);

        Task<ProductsViewModel<CatalogProductViewModel>> GetSearchCatalogAsync(string keyword);

        Task<IEnumerable<CatalogProductViewModel>> FilterCategoryCatalogAsync(string categoryName, string filterJson);

        Task<IEnumerable<CatalogProductViewModel>> FilterSearchCatalogAsync(string keyword, string filterJson);

        Task<ProductDetailsModel> GetProductDetails(int productId);

        Task<bool> IsProductInDbFavorites(string customerId, int productId);

        Task<bool> CategoryExistsAsync(string categoryName);
    }
}
