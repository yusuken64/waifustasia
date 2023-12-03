using Azure.Storage.Blobs;
using Newtonsoft.Json;
using System.Text;
using Waifustasia.Data.Authentication;

namespace Waifustasia.WaifuAI.Settings
{
    public class AzureSettingStore<T> where T : new()
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _settingFileName;

        public AzureSettingStore(IConfiguration configuration)
        {
            CryptoString cryptoString = new CryptoString();
            _connectionString = cryptoString.DecryptSettingString(configuration["AppSettings:AzureRecord:BlobConnectionString"]);
            _containerName =    configuration["AppSettings:AzureRecord:SettingContainer"];
            _settingFileName =  configuration["AppSettings:AzureRecord:SettingFileName"];
        }

        public async Task SaveSettingsAsync(T settings)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(_settingFileName);

            var serializedSettings = JsonConvert.SerializeObject(settings);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedSettings)))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }

        public async Task<T> GetSettingsAsync()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(_settingFileName);

            if (!await blobClient.ExistsAsync())
            {
                return new();
            }

            var downloadResponse = await blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(downloadResponse.Value.Content))
            {
                var serializedSettings = await streamReader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(serializedSettings);
            }
        }
    }
}
