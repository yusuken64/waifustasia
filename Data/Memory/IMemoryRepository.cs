namespace Waifustasia.Data.Memory
{
	public interface IMemoryRepository
	{
		Task<Memory> CreateMemoryAsync(Memory newMemory);
		Task<Memory?> GetMemoryByIdAsync(string id);
		Task<List<Memory>?> GetMemoryByWaifuIdAsync(string waifuId);
		Task<List<Memory>> GetMemoryFeedAsync(User.User user, int pageIndex, int pageSize);
		Task<List<Memory>> GetPublicMemoryFeedAsync(int pageIndex, int pageSize);
		Task IncrementViewsAsync(Memory memory);
        Task LikeMemoryAsync(Memory memory);
        Task LoadMemoryUserAsync(Memory memory);
    }
}