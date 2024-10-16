namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Details;

    public interface IDetailsService
    {
        Task<ProductDetailsModel> GetProductDetails(int productId);

        Task<bool> IsProductInDbFavorites(string customerId, int productId);

        Task<bool> IsProductInSessionFavorites(ICollection<int> sessionFavorites, int productId);
    }
}
