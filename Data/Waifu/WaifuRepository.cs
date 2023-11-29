using Microsoft.EntityFrameworkCore;

namespace Waifustasia.Data.Waifu;

public class WaifuRepository : IWaifuRepository
{
    private readonly WaifustasiaDbContext _context; // Your database context

    public WaifuRepository(WaifustasiaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Waifu>> GetAllWaifusAsync()
    {
        return await _context.Waifus.ToListAsync();
    }

    public async Task<Waifu?> GetWaifuByIdAsync(string id)
    {
        return await _context.Waifus.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Waifu> CreateWaifuAsync(Waifu newWaifu)
    {
		var existingWaifu = await _context.Waifus.FirstOrDefaultAsync(w => w.Id == newWaifu.Id);

        if (existingWaifu == null)
        {
            _context.Waifus.Add(newWaifu);
            await _context.SaveChangesAsync();
        }
        else
		{
            existingWaifu.UserId = newWaifu.UserId;
			existingWaifu.Creator   = newWaifu.Creator;
            existingWaifu.Name = newWaifu.Name;
            existingWaifu.ProfileDescription = newWaifu.ProfileDescription;
            existingWaifu.PersonalityPrompt = newWaifu.PersonalityPrompt;
            existingWaifu.ImageUrl = newWaifu.ImageUrl;
            existingWaifu.Followers = newWaifu.Followers;
            existingWaifu.Memories = newWaifu.Memories;
            existingWaifu.Score = newWaifu.Score;

			_context.Waifus.Update(existingWaifu);
			await _context.SaveChangesAsync();
        }
        return newWaifu;
    }

    public async Task<Waifu> UpdateWaifuAsync(Waifu updatedWaifu)
    {
        _context.Waifus.Update(updatedWaifu);
        await _context.SaveChangesAsync();
        return updatedWaifu;
    }

    public async Task<bool> DeleteWaifuAsync(string id)
    {
        var waifu = await _context.Waifus.FirstOrDefaultAsync(w => w.Id == id);
        if (waifu != null)
        {
            _context.Waifus.Remove(waifu);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<int> GetFollowerCountAsync(string waifuId)
    {
        return await _context.UserWaifuFollows.CountAsync(x => x.WaifuId == waifuId);
    }
}