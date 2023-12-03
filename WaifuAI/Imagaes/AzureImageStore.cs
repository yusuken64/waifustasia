using Azure.Storage.Blobs;
using Waifustasia.Data.Authentication;

namespace Waifustasia.WaifuAI.Imagaes
{
    public class AzureImageStore : IImageStore
    {
        string _connectionString;
        string _containerName;

        public AzureImageStore(IConfiguration configuration)
        {
            CryptoString cryptoString= new CryptoString();
            _connectionString = cryptoString.DecryptSettingString(configuration["AppSettings:AzureRecord:BlobConnectionString"]);
            _containerName = configuration["AppSettings:AzureRecord:ImageContainer"];
        }

        public async Task<string> SaveImageAsync(string imageUrl)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Create the container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();

            var blobName = Path.GetFileName(imageUrl);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (var httpClient = new HttpClient())
            {
                // Download the image from the URL
                var response = await httpClient.GetAsync(imageUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to download the image from the URL.");
                }

                // Upload the downloaded image to the blob
                await using var stream = await response.Content.ReadAsStreamAsync();
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL of the uploaded blob
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
