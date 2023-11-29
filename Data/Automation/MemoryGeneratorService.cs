using Waifustasia.WaifuAI.Settings;

namespace Waifustasia.Data.Automation
{
    public class MemoryGeneratorService : BackgroundService
    {
        public MemoryGeneratorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private readonly IServiceScopeFactory _scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            AzureSettingStore<AzureRecord> azureSettings;
            using (var scope = _scopeFactory.CreateScope())
            {
                azureSettings = scope.ServiceProvider.GetRequiredService<AzureSettingStore<AzureRecord>>();
            }
            
            var settings = await azureSettings.GetSettingsAsync();
            if (settings?.LastAutoGeneration == null)
            {
                settings = new AzureRecord()
                {
                    LastAutoGeneration = DateTime.UtcNow - TimeSpan.FromHours(25)
                };
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var timeSinceLastGeneration = DateTime.UtcNow - (settings.LastAutoGeneration ?? DateTime.UtcNow);
                if (timeSinceLastGeneration.TotalHours < 24)
                {
                    var delayDuration = TimeSpan.FromHours(24) - timeSinceLastGeneration;
                    await Task.Delay(delayDuration, stoppingToken);
                }

                using var scope = _scopeFactory.CreateScope();
                var memoryGenerator = scope.ServiceProvider.GetRequiredService<MemoryGenerator>();

                // Invoke your process here
                await memoryGenerator.GenerateAutoMemories();

                settings.LastAutoGeneration = DateTime.UtcNow;
                await azureSettings.SaveSettingsAsync(settings);

                // Wait for 24 hours before running again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
