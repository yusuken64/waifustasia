namespace Waifustasia.Data.Waifu;

public interface IWaifuRepository
{
    Task<List<Waifu>> GetAllWaifusAsync();
    Task<Waifu?> GetWaifuByIdAsync(string id);
    Task<Waifu> CreateWaifuAsync(Waifu newWaifu);
    Task<Waifu> UpdateWaifuAsync(Waifu updatedWaifu);
    Task<bool> DeleteWaifuAsync(string id);
    Task<int> GetFollowerCountAsync(string waifuId);
}
