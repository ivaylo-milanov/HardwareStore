namespace HardwareStore.Core.Services.Contracts
{
    public interface IDropboxService
    {
        Task<string> GetProductFirstImageAsync(int productId);

        Task<IEnumerable<string>> GetAllProductImagesAsync(int productId);
    }
}
