namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task AddToSessionShoppingCartAsync(int id, int quantity);

        Task RemoveFromSessionShoppingCartAsync(int id);

        Task<ShoppingCartViewModel> GetSessionShoppingCartAsync();

        Task DecreaseSessionItemQuantityAsync(int productId);

        Task AddToDatabaseShoppingCartAsync(int productId, int quantity);

        Task RemoveFromDatabaseShoppingCartAsync(int productId);

        Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync();

        Task DecreaseDatabaseItemQuantityAsync(int productId);
    }
}
