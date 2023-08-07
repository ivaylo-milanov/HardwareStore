namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCart;

    public interface IShoppingCartService
    {
        Task AddToDatabaseShoppingCartAsync(int productId, int quantity, string userId);

        Task<ICollection<ShoppingCartExportModel>> AddToSessionShoppingCartAsync(int productId, int quantity, ICollection<ShoppingCartExportModel> cart);

        Task RemoveFromDatabaseShoppingCartAsync(int productId, string userId);

        Task<ICollection<ShoppingCartExportModel>> RemoveFromSessionShoppingCartAsync(int productId, ICollection<ShoppingCartExportModel> cart);

        Task<ShoppingCartViewModel> GetDatabaseShoppingCartAsync(string userId);

        Task<ShoppingCartViewModel> GetSessionShoppingCartAsync(ICollection<ShoppingCartExportModel> cart);

        Task DecreaseDatabaseItemQuantityAsync(int productId, string userId);

        Task<ICollection<ShoppingCartExportModel>> DecreaseSessionItemQuantityAsync(int productId, ICollection<ShoppingCartExportModel> cart);

        Task IncreaseDatabaseItemQuantityAsync(int productId, string userId);

        Task<ICollection<ShoppingCartExportModel>> IncreaseSessionItemQuantityAsync(int productId, ICollection<ShoppingCartExportModel> cart);

        Task UpdateDatabaseItemQuantityAsync(int quantity, int productId, string userId);

        Task<ICollection<ShoppingCartExportModel>> UpdateSessionItemQuantityAsync(int quantity, int productId, ICollection<ShoppingCartExportModel> cart);
    }
}
