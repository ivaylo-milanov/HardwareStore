namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task AddToDatabaseShoppingCartAsync(int productId, int quantity, string userId);

        Task RemoveFromDatabaseShoppingCartAsync(int productId, string userId);

        Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync(string userId);

        Task DecreaseDatabaseItemQuantityAsync(int productId, string userId);

        Task IncreaseDatabaseItemQuantityAsync(int productId, string userId);

        Task UpdateDatabaseItemQuantityAsync(int quantity, int productId, string userId);
    }
}
