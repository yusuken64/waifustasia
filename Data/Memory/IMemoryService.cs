namespace Waifustasia.Data.Memory
{
	public interface IMemoryService
	{
		Task<Memory> GenerateMemoryAsync(Chat.Chat chat);
        Task<List<Memory>?> GetMemoriesByWaifuId(string waifuId);
		Task<Memory?> GetMemoryByIdAsync(string id);
		Task<List<Memory>> GetMemoryFeedAsync(User.User user, int pageIndex, int pageSize);
		Task<List<Memory>> GetPublicMemoryFeedAsync(int pageIndex, int pageSize);
		Task IncrementViewsAsync(Memory memory);
        Task LikeMemoryAsync(Memory memory);
        Task LoadMemoryUserAsync(Memory memory);
    }
}