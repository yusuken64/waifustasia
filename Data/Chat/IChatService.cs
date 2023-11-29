namespace Waifustasia.Data.Chat
{
    using User = User.User;
    using Waifu = Waifu.Waifu;

    public interface IChatService
    {
        Task<Chat> OpenChatAsync(User user, Waifu waifu);
        Task AppendChatAsync(int chatId, string senderId, Message message);
        Message CreateMessage(Chat chat, string senderID, string newMessage);
        Task<Message> CreateReplyMessageAsync(Chat chat, string senderID, string newMessage);
        Task<List<PopularWaifusAndChatsResult>> GetMostPopularWaifusAndChatsAsync(int hoursAgo);
    }
}
