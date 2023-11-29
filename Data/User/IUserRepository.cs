namespace Waifustasia.Data.User;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<User> CreateUserAsync(User newUser);
    Task<User> UpdateUserAsync(User updatedUser);
	Task<User> UpdateUserNameAsync(string userId, string newDisplayName);
    Task<bool> DeleteUserAsync(string id);
    Task<User?> GetUserByEmailAndPasswordAsync(string username, string password);
    Task<bool> HasLikedMemory(string userId, string memoryId);
    Task AddLikedMemory(string userId, string memoryId);
	Task<List<Memory.Memory>> GetLikedMemoriesAsync(string userId);
    Task FollowAsnyc(string userId, string waifuId);
    Task UnFollowAsnyc(string userId, string waifuId);
    Task<bool> GetFollowStatusAsyncc(string userId, string waifuId);
	Task UpdateUserRoleAsync(string userID, string role);
}
