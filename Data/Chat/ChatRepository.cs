using Microsoft.EntityFrameworkCore;

namespace Waifustasia.Data.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly WaifustasiaDbContext dbContext;

        public ChatRepository(WaifustasiaDbContext context)
        {
            dbContext = context;
        }

        public async Task<Chat?> OpenChatAsync(User.User user, Waifu.Waifu waifu)
        {
            if (user == null || waifu == null)
            {
                return null;
            }

            var existingChat = await dbContext.Chats
                .Include(x => x.User)
                .Include(x => x.Waifu)
				.Include(x => x.Messages)
				.FirstOrDefaultAsync(x => x.UserId == user.Id && x.WaifuId == waifu.Id);

            if (existingChat != null)
            {
                return existingChat; // Return existing chat if found
            }

            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            var existingWaifu = await dbContext.Waifus.FirstOrDefaultAsync(x => x.Id == waifu.Id);

            // Create a new chat between the user and waifu
            var newChat = new Chat
            {
                UserId = existingUser.Id,
                User = existingUser,
                WaifuId = existingWaifu.Id,
                Waifu = existingWaifu,
                Messages = new List<Message>()
            };

            dbContext.Chats.Add(newChat);
            await dbContext.SaveChangesAsync();

            return newChat;
        }

        public async Task AppendChatAsync(int chatId, string senderId, Message message)
        {
            var chat = await dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (chat != null)
            {
                chat.Messages.Add(message);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<PopularWaifusAndChatsResult>> GetMostPopularWaifusAndChatsAsync(int hoursAgo)
        {
            DateTime nHoursAgo = DateTime.UtcNow.AddHours(-hoursAgo);

            var popularWaifus = await dbContext.Messages
                .Where(msg => msg.Timestamp >= nHoursAgo)
                .Include(msg => msg.Chat)
                .GroupBy(msg => msg.Chat.WaifuId)
                .Select(g => new
                {
                    WaifuId = g.Key,
                    MessageCount = g.Count(),
                    MostRecentMessage = g.OrderByDescending(m => m.Timestamp).FirstOrDefault()
                })
                .OrderByDescending(x => x.MessageCount)
                .Take(10)
                .ToListAsync();

            var waifuIds = popularWaifus.Select(w => w.WaifuId).ToList();

            var mostActiveChats = popularWaifus.Select(w => new PopularWaifusAndChatsResult
            {
                Waifu = dbContext.Waifus.FirstOrDefault(waifu => waifu.Id == w.WaifuId),
                MessageCount = w.MessageCount,
                MostActiveChat = w.MostRecentMessage != null ? w.MostRecentMessage.Chat : null
            }).ToList();

            return mostActiveChats;
        }
    }

    public class PopularWaifusAndChatsResult
    {
        public Waifu.Waifu? Waifu { get; init; }
        public int MessageCount { get; set; }
        public Chat? MostActiveChat { get; init; }
    }
}