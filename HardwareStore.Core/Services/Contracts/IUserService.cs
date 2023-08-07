namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Core.ViewModels.ShoppingCart;
    using HardwareStore.Infrastructure.Models;

    public interface IUserService
    {
        Task<ICollection<Favorite>> GetCustomerFavorites(string userId);

        Task<ICollection<ShoppingCartItem>> GetCustomerShoppingCart(string userId);

        Task<ProfileViewModel> GetCustomerProfile(string userId);

        Task AddToDatabase(string userId, ICollection<int> favorites, ICollection<ShoppingCartExportModel> cart);

        Task<Customer> GetCustomerWithShoppingCart(string userId);

        Task<Customer> GetCustomerWithOrders(string userId);

        Task<Customer> GetCustomerWithFavorites(string userId);

        Task<bool> IsFavorite(int productId, string userId, ICollection<int> favorites);
    }
}
