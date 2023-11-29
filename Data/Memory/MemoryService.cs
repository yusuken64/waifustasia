using Waifustasia.OpenAI;
using Waifustasia.WaifuAI;

namespace Waifustasia.Data.Memory
{
    public class MemoryService : IMemoryService
	{
		private readonly IMemoryRepository memoryRepository;
		private readonly IIntellegenceService intellegenceService;
        private readonly IImageStore imageStore;

        public MemoryService(IMemoryRepository memoryRepository,
			IIntellegenceService intellegenceService,
			IImageStore imageStore)
		{
			this.memoryRepository = memoryRepository;
			this.intellegenceService = intellegenceService;
            this.imageStore = imageStore;
        }

		public async Task<Memory> GenerateMemoryAsync(Chat.Chat chat)
		{
			var recentChat = chat.Messages.OrderByDescending(x => x.Timestamp).Take(10).ToArray();

			var dateDescription = await intellegenceService.GenerateDateDescription(recentChat);
			var memoryImagePrompt = await intellegenceService.GenerateMemoryImagePrompt(dateDescription);
			var memoryDescription = await intellegenceService.GenerateMemoryDescription(dateDescription);
			var dateImageUrl = (await intellegenceService.GenerateImage(memoryImagePrompt))[0];

			var savedImageUrl = await imageStore.SaveImageAsync(dateImageUrl);

			var memory = new Memory()
			{
				CreationDate = DateTime.UtcNow,
				GenerationPrompt = dateDescription,
				MemoryDescription = memoryDescription,
				MemoryImageUrl = savedImageUrl,
				MemoryUserId = chat.UserId,
				MemoryUser = chat.User,
				WaifuId = chat.WaifuId.ToString(),
				Waifu = chat.Waifu
			};

			var newMemory = await memoryRepository.CreateMemoryAsync(memory);

			return newMemory;
		}

		public async Task<List<Memory>?> GetMemoriesByWaifuId(string waifuId)
		{
			return await memoryRepository.GetMemoryByWaifuIdAsync(waifuId);
		}

        public async Task<Memory?> GetMemoryByIdAsync(string id)
        {
			return await memoryRepository.GetMemoryByIdAsync(id);
        }

        public async Task LoadMemoryUserAsync(Memory memory)
        {
            await memoryRepository.LoadMemoryUserAsync(memory);
        }

        public async Task IncrementViewsAsync(Memory memory)
        {
            await memoryRepository.IncrementViewsAsync(memory);
        }

        public async Task LikeMemoryAsync(Memory memory)
        {
            await memoryRepository.LikeMemoryAsync(memory);
        }

		public async Task<List<Memory>> GetMemoryFeedAsync(User.User user, int pageIndex, int pageSize)
		{
			return await memoryRepository.GetMemoryFeedAsync(user, pageIndex, pageSize);
		}

		public async Task<List<Memory>> GetPublicMemoryFeedAsync(int pageIndex, int pageSize)
		{
			return await memoryRepository.GetPublicMemoryFeedAsync(pageIndex, pageSize);
		}
	}
}
