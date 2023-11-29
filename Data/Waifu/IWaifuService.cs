namespace Waifustasia.Data.Waifu;

public interface IWaifuService
{
    Task<Waifu> CreateWaifuAsync(Waifu newWaifu);
    Task<bool> DeleteWaifuAsync(string id);
    Task<Waifu?> GetWaifuByIdAsync(string id);
    Task<List<Waifu>> GetWaifusAsync();
    Task<Waifu> UpdateWaifuAsync(Waifu updatedWaifu);
    Task IncrementScore(Waifu waifu, int score);
    Task<int> GetFollowerCountAsync(string waifuId);
}