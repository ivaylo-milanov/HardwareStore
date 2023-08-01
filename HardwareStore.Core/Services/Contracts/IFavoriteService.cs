namespace HardwareStore.Core.Services.Contracts
{
    public interface IFavoriteService
    {
        Task AddToSessionFavoriteAsync(int productId);

        Task AddToDatabaseFavoriteAsync(int productId);

        Task RemoveFromSessionFavoriteAsync(int productId);

        Task RemoveFromDatabaseFavoriteAsync(int productId);
    }
}
