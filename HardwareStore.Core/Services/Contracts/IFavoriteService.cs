namespace HardwareStore.Core.Services.Contracts
{
    public interface IFavoriteService
    {
        Task<List<int>> AddToFavoriteAsync(List<int> favorites, int id);

        Task<List<int>> RemoveFromFavoriteAsync(List<int> favorites, int id)
    }
}
