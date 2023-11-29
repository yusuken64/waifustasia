namespace Waifustasia.Data.User
{
    public interface IUserService
    {
        Task ClearBrowserUserDataAsync();
        Task<User> CreateUserAsync(User newUser);
        Task<bool> DeleteUserAsync(string id);
        Task<User?> FetchUserFromBrowserAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        Task PersistUserToBrowserAsync(User user);
        Task<User> UpdateUserAsync(User updatedUser);
        Task IncrementRizz(User user, int score);
		Task<User> UpdateUserNameAsync(string id, string userName);
        Task<bool> HasLikedMemory(string userId, string memoryId);
        Task AddLikedMemory(string userId, string memoryId);
		Task<List<Memory.Memory>> GetLikedMemoriesAsync(string userId);
        Task FollowByIdAync(string userId, string waifuId);
        Task UnFollowByIdAync(string userId, string waifuId);
        Task<bool> GetFollowStatusAsync(string userId, string waifuId);
		Task UpdateUserRoleAsync(string userID, string role);
	}
}