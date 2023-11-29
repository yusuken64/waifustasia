using Microsoft.EntityFrameworkCore;

namespace Waifustasia.Data.Memory
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly WaifustasiaDbContext _context; // Your database context

        public MemoryRepository(WaifustasiaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Memory>?> GetMemoryByWaifuIdAsync(string waifuId)
        {
            return await _context.Memories.Where(m => m.WaifuId == waifuId).ToListAsync();
        }

        public async Task<Memory?> GetMemoryByIdAsync(string id)
        {
            return await _context.Memories
                .Include(x => x.Waifu)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Memory> CreateMemoryAsync(Memory newMemory)
        {
            newMemory.MemoryUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == newMemory.MemoryUserId);
            newMemory.Waifu = await _context.Waifus.FirstOrDefaultAsync(x => x.Id == newMemory.WaifuId);

			_context.Memories.Add(newMemory);
            await _context.SaveChangesAsync();
            return newMemory;
        }

        public async Task LoadMemoryUserAsync(Memory memory)
        {
            if (memory != null)
            {
                await _context.Entry(memory)
                    .Reference(m => m.MemoryUser)
                    .LoadAsync();
            }
        }

        public async Task IncrementViewsAsync(Memory memory)
        {
            if (memory != null)
            {
                memory.Views++; // Increment views count
                _context.Update(memory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task LikeMemoryAsync(Memory memory)
        {
            if (memory != null)
            {
                memory.Likes++; // Increment views count
                _context.Update(memory);
                await _context.SaveChangesAsync();
            }
        }

		public async Task<List<Memory>> GetMemoryFeedAsync(User.User user, int pageIndex, int pageSize)
		{
			if (user == null || !user.FollowedWaifus.Any())
			{
				return new List<Memory>(); // Return an empty list if the user has no followed waifus or if the user is null
			}

			var waifuIds = user.FollowedWaifus.Select(w => w.WaifuId).ToList();

			var memoriesForWaifus = await _context.Memories
				.Where(memory => waifuIds.Contains(memory.WaifuId))
				.OrderByDescending(memory => memory.CreationDate)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return memoriesForWaifus;
		}

		public async Task<List<Memory>> GetPublicMemoryFeedAsync(int pageIndex, int pageSize)
		{
			var memoriesForWaifus = await _context.Memories
				.OrderByDescending(memory => memory.CreationDate)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return memoriesForWaifus;
		}
	}
}