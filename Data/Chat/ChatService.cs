namespace Waifustasia.Data.Chat
{
    using System.Collections.Generic;
    using Waifustasia.OpenAI;
    using User = User.User;
    using Waifu = Waifu.Waifu;

    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;
        private readonly IIntellegenceService intellegenceService;

        public ChatService(IChatRepository chatRepository, IIntellegenceService intellegenceService)
        {
            this.chatRepository = chatRepository;
            this.intellegenceService = intellegenceService;
        }

        public async Task<Chat> OpenChatAsync(User user, Waifu waifu)
        {
            return await chatRepository.OpenChatAsync(user, waifu);
        }

        public async Task AppendChatAsync(int chatId, string senderId, Message message)
        {
            await chatRepository.AppendChatAsync(chatId, senderId, message);
        }

        public Message CreateMessage(Chat chat, string senderID, string newMessage)
        {
            return new Message()
            {
                Chat = chat,
                ChatId = chat.ChatId,
                Content = newMessage,
                Timestamp = DateTime.UtcNow,
                SentByUser = true
            };
        }

        public async Task<Message> CreateReplyMessageAsync(Chat chat, string senderID, string newMessage)
        {
            var replyMessage = await GenerateReplyAsync(chat.Waifu, chat, newMessage); // Replace with your actual logic

            return new Message()
            {
                Chat = chat,
                ChatId = chat.ChatId,
                Content = replyMessage,
                Timestamp = DateTime.UtcNow,
                SentByUser = false
            };
        }
        private async Task<string> GenerateReplyAsync(Waifu waifu, Chat chat, string message)
        {
            var chatHistory = chat.Messages.ToList();
            chatHistory.Add(new Data.Chat.Message()
            {
                Content = message,
                SentByUser = true,
            });

            var response = await intellegenceService.GetWaifuResponse(waifu, chatHistory.ToArray());
            return response;
        }

        public async Task<List<PopularWaifusAndChatsResult>> GetMostPopularWaifusAndChatsAsync(int hoursAgo)
        {
            return await chatRepository.GetMostPopularWaifusAndChatsAsync(hoursAgo);
        }
    }
}
