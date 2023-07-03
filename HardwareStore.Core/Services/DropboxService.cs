namespace HardwareStore.Core.Services
{
    using Dropbox.Api;
    using Dropbox.Api.Files;
    using HardwareStore.Core.Services.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DropboxService : IDropboxService
    {
        private DropboxClient dropboxClient;

        public DropboxService(string accessToken)
        {
            dropboxClient = new DropboxClient(accessToken);
        }

        public async Task<IEnumerable<string>> GetAllProductImagesAsync(int productId)
        {
            var searchResult = await dropboxClient.Files.SearchAsync(string.Empty, productId.ToString());

            var imageUrls = new List<string>();

            foreach (var match in searchResult.Matches)
            {
                if (match.Metadata.IsFile)
                {
                    var fileMetadata = match.Metadata as FileMetadata;
                    var linkResult = await dropboxClient.Files.GetTemporaryLinkAsync(fileMetadata.PathLower);
                    imageUrls.Add(linkResult.Link);
                }
            }

            return imageUrls;
        }

        public async Task<string> GetProductFirstImageAsync(int productId)
        {
            var searchResult = await dropboxClient.Files.SearchAsync(string.Empty, productId.ToString());
            var fileMetadata = searchResult.Matches.FirstOrDefault(m => m.Metadata.IsFile);

            if (fileMetadata != null)
            {
                var linkResult = await dropboxClient.Files.GetTemporaryLinkAsync(fileMetadata.Metadata.PathLower);
                return linkResult.Link;
            }

            return null;
        }
    }
}