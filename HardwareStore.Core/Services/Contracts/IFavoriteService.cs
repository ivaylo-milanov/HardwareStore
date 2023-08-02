namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Favorite;

    public interface IFavoriteService
    {
        Task<ICollection<FavoriteExportModel>> GetDatabaseFavoriteAsync();

        Task<ICollection<FavoriteExportModel>> GetSessionFavoriteAsync();

        Task AddToSessionFavoriteAsync(int productId);

        Task AddToDatabaseFavoriteAsync(int productId);

        Task RemoveFromSessionFavoriteAsync(int productId);

        Task RemoveFromDatabaseFavoriteAsync(int productId);
        Task<bool> IsFavorite(int productId);
    }
}
