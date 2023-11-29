using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace Waifustasia.Data.User;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly ProtectedLocalStorage protectedLocalStorage;
    private readonly string _userStorageKey = "userStorageKey";

    public UserService(IUserRepository userRepository,
        ProtectedLocalStorage protectedLocalStorage)
    {
        this.userRepository = userRepository;
        this.protectedLocalStorage = protectedLocalStorage;
    }

    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        await protectedLocalStorage.SetAsync(_userStorageKey, userJson);
    }

    public async Task<User?> FetchUserFromBrowserAsync()
    {
        try
        {
            var storedUserResult = await protectedLocalStorage.GetAsync<string>(_userStorageKey);

            if (storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
            {
                var user = JsonConvert.DeserializeObject<User>(storedUserResult.Value);

                return user;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex}");
        }

        return null;
    }

    public async Task ClearBrowserUserDataAsync() => await protectedLocalStorage.DeleteAsync(_userStorageKey);

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(User newUser)
    {
        // Optional: Add additional logic/validation before creating the user
        return await userRepository.CreateUserAsync(newUser);
    }

    public async Task<User> UpdateUserAsync(User updatedUser)
    {
        // Optional: Add additional logic/validation before updating the user
        return await userRepository.UpdateUserAsync(updatedUser);
    }

    public async Task<User> UpdateUserNameAsync(string id, string userName)
    {
        return await userRepository.UpdateUserNameAsync(id, userName);
    }

	public async Task<bool> DeleteUserAsync(string id)
    {
        // Optional: Add additional logic/validation before deleting the user
        return await userRepository.DeleteUserAsync(id);
    }

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
    {
        return await userRepository.GetUserByEmailAndPasswordAsync(email, password);
    }

	public async Task IncrementRizz(User user, int score)
	{
        user.Rizz += score;
        await userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> HasLikedMemory(string userId, string memoryId)
    {
        return await userRepository.HasLikedMemory(userId, memoryId);
    }

    public async Task AddLikedMemory(string userId, string memoryId)
    {
        await userRepository.AddLikedMemory(userId, memoryId);
    }

    public async Task<List<Memory.Memory>> GetLikedMemoriesAsync(string userId)
	{
		return await userRepository.GetLikedMemoriesAsync(userId);
	}

    public async Task FollowByIdAync(string userId, string waifuId)
    {
        await userRepository.FollowAsnyc(userId, waifuId);
    }

    public async Task UnFollowByIdAync(string userId, string waifuId)
    {
        await userRepository.UnFollowAsnyc(userId, waifuId);
    }

    public async Task<bool> GetFollowStatusAsync(string userId, string waifuId)
    {
        return await userRepository.GetFollowStatusAsyncc(userId, waifuId);
    }

    public async Task UpdateUserRoleAsync(string userID, string role)
    {
        await userRepository.UpdateUserRoleAsync(userID, role);
    }
}