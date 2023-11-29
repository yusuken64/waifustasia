using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static Waifustasia.Data.User.User;

namespace Waifustasia.Data.User;

public class UserRepository : IUserRepository
{
    private readonly WaifustasiaDbContext dbContext;

    public UserRepository(WaifustasiaDbContext context)
    {
        dbContext = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        var first = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return first;
    }

    public async Task<User> CreateUserAsync(User newUser)
    {
        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();
        return newUser;
    }

    public async Task<User> UpdateUserAsync(User updatedUser)
    {
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == updatedUser.Id);

        if (existingUser != null)
        {
            // Update properties of the existingUser entity with the values from updatedUser
            existingUser.DisplayName = updatedUser.DisplayName;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;
            existingUser.CreatedWaifus = updatedUser.CreatedWaifus;
            existingUser.Coin = updatedUser.Coin;
            existingUser.Rizz = updatedUser.Rizz;
            existingUser.Role = updatedUser.Role;

            await dbContext.SaveChangesAsync();
        }

        return existingUser;
    }

    public async Task<User> UpdateUserNameAsync(string userId, string newDisplayName)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null)
        {
            user.DisplayName = newDisplayName;
            await dbContext.SaveChangesAsync();
        }

        return user;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
    }


    public async Task<bool> HasLikedMemory(string userId, string memoryId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        return user?.LikedMemories?.Any(m => m.Id == memoryId) ?? false;
    }

    public async Task AddLikedMemory(string userId, string memoryId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        var memory = await dbContext.Memories.FindAsync(memoryId);

        if (user != null && memory != null)
        {
            user.LikedMemories ??= new List<Memory.Memory>();
            user.LikedMemories.Add(memory);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Memory.Memory>> GetLikedMemoriesAsync(string userId)
    {
        var user = await dbContext.Users
                .Include(u => u.LikedMemories) // Include LikedMemories navigation property
                .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.LikedMemories.ToList() ?? new List<Memory.Memory>();
    }

    public async Task FollowAsnyc(string userId, string waifuId)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId); // Retrieve user
        var waifu = await dbContext.Waifus.FirstOrDefaultAsync(w => w.Id == waifuId); // Retrieve channel

        if (user != null && waifu != null)
        {
            var following = await dbContext.UserWaifuFollows.FirstOrDefaultAsync(u => u.UserId == user.Id && u.WaifuId == waifu.Id);
            if (following == null)
            {
                var subscription = new UserWaifuFollow
                {
                    User = user,
                    UserId = user.Id,
                    Waifu = waifu,
                    WaifuId = waifuId
                };

                waifu.Followers.Add(subscription);

                dbContext.UserWaifuFollows.Add(subscription);
                dbContext.Waifus.Update(waifu);
                dbContext.SaveChanges();
            }
            else
            {
                following.User = user;
                following.UserId = user.Id;
                following.Waifu = waifu;
                following.WaifuId = waifu.Id;

                if (!waifu.Followers.Contains(following))
                {
                    waifu.Followers.Add(following);
                }

                dbContext.UserWaifuFollows.Update(following);
                dbContext.Waifus.Update(waifu);
                dbContext.SaveChanges();
            }
        }
    }

    public async Task UnFollowAsnyc(string userId, string waifuId)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId); // Retrieve user
        var waifu = await dbContext.Waifus.FirstOrDefaultAsync(w => w.Id == waifuId); // Retrieve channel

        var subscription = await dbContext.UserWaifuFollows.FirstOrDefaultAsync(x => x.UserId == userId && x.WaifuId == waifuId);

        if (subscription != null)
        {
            var first = waifu.Followers.FirstOrDefault(x => x.WaifuId == waifu.Id && x.UserId == user.Id);
            if (first != null)
            {
                waifu.Followers.Remove(first);
                dbContext.Waifus.Update(waifu);
                dbContext.SaveChanges();
            }

            //dbContext.UserWaifuFollows.Remove(subscription);
            //dbContext.SaveChanges();
        }
    }

    public async Task<bool> GetFollowStatusAsyncc(string userId, string waifuId)
    {
        var following = await dbContext.UserWaifuFollows.FirstOrDefaultAsync(u => u.UserId == userId && u.WaifuId == waifuId);
        return following != null;
    }

    [Authorize(Roles = "Admin")]
    public async Task UpdateUserRoleAsync(string userId, string role)
    {
        var validRoles = new List<string> { "Admin", "User", "SuperUser" };
        if (!validRoles.Contains(role))
        {
            return; // or throw an exception, return BadRequest(), etc.
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null &&
            user.Role != "Admin")
        {
            user.Role = role;
            await dbContext.SaveChangesAsync();
        }
    }
}