namespace HardwareStore.Core.Services.Contracts
{
    public interface IDropboxService
    {
        Task<IEnumerable<byte[]>> GetProductImages(string productId);
    }
}
