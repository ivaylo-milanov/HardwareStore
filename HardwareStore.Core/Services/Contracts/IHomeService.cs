namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Product;

    public interface IHomeService
    {
        Task<IEnumerable<ProductViewModel>> GetNewProducts();

        Task<IEnumerable<ProductViewModel>> GetMostBoughtProducts(); 
    }
}
