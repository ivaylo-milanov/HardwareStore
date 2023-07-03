namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;

    public interface IProductService<TModel> 
        where TModel : ProductViewModel
    {
        Task<IEnumerable<TModel>> GetAllProducts();
    }
}
