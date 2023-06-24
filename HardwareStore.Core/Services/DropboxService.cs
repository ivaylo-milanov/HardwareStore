namespace HardwareStore.Core.Services
{
    using Dropbox.Api;
    using HardwareStore.Core.Services.Contracts;

    public class DropboxService : IDropboxService
    {
        private DropboxClient dropboxClient;

        public DropboxService(string accessToken)
        {
            dropboxClient = new DropboxClient(accessToken);
        }

        public async Task<IEnumerable<byte[]>> GetProductImages(string productId)
        {
            var folderPath = $"/products/{productId}";

            var images = new List<byte[]>();

            try
            {
                var listFolderResult = await dropboxClient.Files.ListFolderAsync(folderPath);

                foreach (var entry in listFolderResult.Entries)
                {
                    if (entry.IsFile && IsImageFile(entry.Name))
                    {
                        var downloadResponse = await dropboxClient.Files.DownloadAsync(entry.PathLower);
                        var imageData = await downloadResponse.GetContentAsByteArrayAsync();
                        images.Add(imageData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product images: {ex.Message}");
            }

            return images;
        }

        private bool IsImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
    }
}
