namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Core.ViewModels.Search;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public interface IProductService
    {
        IEnumerable<TModel> FilterProducts<TModel, TFilter>(TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel;

        Task<IEnumerable<SearchViewModel>> GetProductsByKeyword(string keyword);

        Task<ProductsViewModel<SearchViewModel>> GetSearchModel(string keyword);

        Task<ProductDetailsModel> GetProductDetails(int id);

        Task<ProductsViewModel<TModel>> GetModel<TModel>() where TModel : ProductViewModel;
    }
}
