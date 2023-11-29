namespace Waifustasia.Data.Waifu;

public class WaifuService : IWaifuService
{
    private readonly IWaifuRepository waifuRepository;

    public WaifuService(IWaifuRepository waifuRepository)
    {
        this.waifuRepository = waifuRepository;
    }

    public async Task<List<Waifu>> GetWaifusAsync()
    {
        return await waifuRepository.GetAllWaifusAsync();
    }

    public async Task<Waifu?> GetWaifuByIdAsync(string id)
    {
        return await waifuRepository.GetWaifuByIdAsync(id);
    }

    public async Task<Waifu> CreateWaifuAsync(Waifu newItem)
    {
        return await waifuRepository.CreateWaifuAsync(newItem);
    }

    public async Task<Waifu> UpdateWaifuAsync(Waifu updatedWaifu)
    {
        return await waifuRepository.UpdateWaifuAsync(updatedWaifu);
    }

    public async Task<bool> DeleteWaifuAsync(string id)
    {
        return await waifuRepository.DeleteWaifuAsync(id);
    }

    public async Task IncrementScore(Waifu waifu, int score)
    {
        var wholeWaifu = await GetWaifuByIdAsync(waifu.Id);
        if (wholeWaifu != null)
        {
            wholeWaifu.Score += score;
            await waifuRepository.UpdateWaifuAsync(wholeWaifu);
        }
	}

    public async Task<int> GetFollowerCountAsync(string waifuId)
    {
        return await waifuRepository.GetFollowerCountAsync(waifuId);
    }
}
