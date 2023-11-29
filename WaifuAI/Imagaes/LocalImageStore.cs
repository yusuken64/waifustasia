namespace Waifustasia.WaifuAI.Imagaes
{
    public class LocalImageStore : IImageStore
    {
        private string? path;
        private readonly HttpClient _httpClient;

        public LocalImageStore(IConfiguration configuration)
        {
            path = configuration["AppSettings:LocalStorage:DirectoryPath"];
            _httpClient = new HttpClient();
        }

        public async Task<string> SaveImageAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    byte[] imageData = await response.Content.ReadAsByteArrayAsync();
                    string fileName = GenerateFileName();
                    string path = Path.Combine(@"C:\Users\yusuk\source\repos\Waifustasia\Waifustasia\wwwroot\Images\", fileName); // Change "YourImagesFolder" to the desired folder path

                    await File.WriteAllBytesAsync(path, imageData);
                    var relativePath = GetRelativePathFromWwwRoot(path);
                    return relativePath;
                }
                else
                {
                    Console.WriteLine($"Failed to download image. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions accordingly (logging, error reporting, etc.)
                Console.WriteLine("Error saving image: " + ex.Message);
                return null;
            }
        }
        public string GetRelativePathFromWwwRoot(string fullPath)
        {
            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!fullPath.StartsWith(wwwRootPath))
            {
                throw new ArgumentException("The file path is not within the wwwroot directory.");
            }

            string relativePath = fullPath.Substring(wwwRootPath.Length).Replace('\\', '/').TrimStart('/');
            return relativePath;
        }

        private string GenerateFileName()
        {
            // Generate a unique file name based on timestamp or other criteria
            return $"{Guid.NewGuid()}.jpg"; // Change the file extension if needed
        }
    }
}
