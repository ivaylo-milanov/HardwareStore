namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Favorite;

    public interface IFavoriteService
    {
        Task<ICollection<FavoriteExportModel>> GetDatabaseFavoriteAsync(string userId);

        Task<ICollection<FavoriteExportModel>> GetSessionFavoriteAsync(ICollection<int> favorites);

        Task<ICollection<int>> AddToSessionFavoriteAsync(int productId, ICollection<int> favorites);

        Task AddToDatabaseFavoriteAsync(int productId, string userId);

        Task<ICollection<int>> RemoveFromSessionFavoriteAsync(int productId, ICollection<int> favorites);

        Task RemoveFromDatabaseFavoriteAsync(int productId, string userId);
    }
}
