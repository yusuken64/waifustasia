using OpenAI.Chat;
using OpenAI.Models;
using Waifustasia.Data.Waifu;

namespace Waifustasia.OpenAI
{
	public interface IIntellegenceService
	{
		Task<string> GenerateDateDescription(Data.Chat.Message[] messages);
		Task<IReadOnlyList<string>> GenerateImage(string prompt);
		Task<string> GenerateMemoryDescription(string dateDescription);
		Task<string> GenerateMemoryImagePrompt(string dateDescription);
		Task<IReadOnlyList<Model>> GetModelsAsync();
		Task<ChatResponse> GetWaifuResponse(Waifu waifu, Data.Chat.Message[] messages);
	}
}