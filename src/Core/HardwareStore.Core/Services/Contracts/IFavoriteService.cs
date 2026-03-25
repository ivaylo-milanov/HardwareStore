namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.Favorite;

    public interface IFavoriteService
    {
        Task<ICollection<FavoriteExportModel>> GetDatabaseFavoriteAsync(string userId);

        Task AddToDatabaseFavoriteAsync(int productId, string userId);

        Task RemoveFromDatabaseFavoriteAsync(int productId, string userId);
    }
}
