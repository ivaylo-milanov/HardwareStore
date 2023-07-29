namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task AddToDatabaseShoppingCartAsync(int productId, int quantity);

        Task AddToSessionShoppingCartAsync(int productId, int quantity);

        Task RemoveFromDatabaseShoppingCartAsync(int productId);

        Task RemoveFromSessionShoppingCartAsync(int productId);

        Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync();

        Task<ShoppingCartViewModel> GetSessionShoppingCartAsync();

        Task DecreaseDatabaseItemQuantityAsync(int productId);

        Task DecreaseSessionItemQuantityAsync(int productId);

        Task IncreaseDatabaseItemQuantityAsync(int productId);

        Task IncreaseSessionItemQuantityAsync(int productId);
    }
}
