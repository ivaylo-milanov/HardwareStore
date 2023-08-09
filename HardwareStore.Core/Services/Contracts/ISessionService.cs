namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCart;

    public interface ISessionService
    {
        Task AddToDatabase(string userId, ICollection<int> favorites, ICollection<ShoppingCartExportModel> cart);
    }
}
