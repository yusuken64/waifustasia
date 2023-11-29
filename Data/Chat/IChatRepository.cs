namespace Waifustasia.Data.Chat
{
    public interface IChatRepository
    {
        Task<Chat?> OpenChatAsync(User.User user, Waifu.Waifu waifu);
        Task AppendChatAsync(int chatId, string senderId, Message message);
        Task<List<PopularWaifusAndChatsResult>> GetMostPopularWaifusAndChatsAsync(int hoursAgo);
    }
}