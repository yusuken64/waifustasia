using Waifustasia.Data.Chat;
using Waifustasia.Data.Memory;

namespace Waifustasia.Data.Automation
{
    public class MemoryGenerator
    {
        public MemoryGenerator(IChatService chatService,
            IMemoryService memoryService)
        {
            ChatService = chatService;
            MemoryService = memoryService;
        }

        public IChatService ChatService { get; }
        public IMemoryService MemoryService { get; }

        public async Task GenerateAutoMemories()
        {
            var popularWaifusAndChats = await ChatService.GetMostPopularWaifusAndChatsAsync(24);

            foreach(var waifuChats in popularWaifusAndChats)
            {
                if (waifuChats.MostActiveChat != null)
                {
                    try
                    {
                        _ = await MemoryService.GenerateMemoryAsync(waifuChats.MostActiveChat);
                    }
                    catch (Exception)
                    {
                        //TODO: log error
                    }
                }
            }
        }
    }
}
