namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Infrastructure.Models;

    public interface IProductService
    {
        Task<IEnumerable<TModel>> GetProductsAsync<TModel>() where TModel : ProductViewModel;

        IEnumerable<TModel> FilterProducts<TModel, TFilter>(IEnumerable<TModel> products, TFilter filter)
            where TFilter : ProductFilterOptions
            where TModel : ProductViewModel;

        Task<IEnumerable<ProductViewModel>> GetProductsByKeyword(string keyword);

        Task<ProductDetailsModel> GetProductDetails(int id);
    }

}
