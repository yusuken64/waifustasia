using OpenAI;
using OpenAI.Images;
using OpenAI.Models;
using Waifustasia.Data.Authentication;
using Waifustasia.Data.Chat;

namespace Waifustasia.OpenAI
{
    public class IntellegenceService : IIntellegenceService
	{
		private OpenAIClient openAiClient;

		public IntellegenceService(IConfiguration configuration)
        {
            CryptoString cryptoString = new CryptoString();
            var key = cryptoString.DecryptSettingString(configuration["AppSettings:OpenAI:key"]);
			openAiClient = new OpenAIClient(key);
		}

		public async Task<IReadOnlyList<Model>> GetModelsAsync()
		{
			IReadOnlyList<Model> models = await openAiClient.ModelsEndpoint.GetModelsAsync();
			return models;
		}

		public async Task<global::OpenAI.Chat.ChatResponse> GetWaifuResponse(Data.Waifu.Waifu waifu, Message[] messages)
		{
			List<global::OpenAI.Chat.Message> openAIMessages = new List<global::OpenAI.Chat.Message>();

			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"You are a Waifu named {waifu.Name}"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"this is your backstory: {waifu.PersonalityPrompt}"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Act as if you are this character, do not mention you are an AI"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Respond as if you are in a conversation in a chat application"));
			var chatHistory = messages.Select(m =>
				new global::OpenAI.Chat.Message(m.SentByUser ? global::OpenAI.Chat.Role.User : global::OpenAI.Chat.Role.Assistant,
												m.Content)).ToList();
			openAIMessages.AddRange(chatHistory);

			global::OpenAI.Chat.ChatRequest chatRequest = new global::OpenAI.Chat.ChatRequest(openAIMessages, "gpt-3.5-turbo-0613");

			global::OpenAI.Chat.ChatResponse responses = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);
			return responses;
		}

		public async Task<string> GenerateDateDescription(Message[] messages)
		{
			List<global::OpenAI.Chat.Message> openAIMessages = new List<global::OpenAI.Chat.Message>();

			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Interpret the conversation as a encounter between two people"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Retell as a narrated story in a novel. focus on the visuals and settings"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Keep it under 3 sentences"));
			var chatHistory = messages.Select(m =>
				new global::OpenAI.Chat.Message(m.SentByUser ? global::OpenAI.Chat.Role.User : global::OpenAI.Chat.Role.Assistant,
												m.Content)).ToList();
			openAIMessages.AddRange(chatHistory);

			global::OpenAI.Chat.ChatRequest chatRequest = new global::OpenAI.Chat.ChatRequest(openAIMessages, "gpt-3.5-turbo-0613");

			global::OpenAI.Chat.ChatResponse responses = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);
			return responses;
		}

		public async Task<string> GenerateMemoryImagePrompt(string dateDescription)
		{
			List<global::OpenAI.Chat.Message> openAIMessages = new List<global::OpenAI.Chat.Message>();

			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"You are an AI for generating Image Prompt"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Given the description of encounter between two people describe an image that would accompany this story. Make it interesting like an instagram photo"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Only respond with the image prompt"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.User, dateDescription));

			global::OpenAI.Chat.ChatRequest chatRequest = new global::OpenAI.Chat.ChatRequest(openAIMessages, "gpt-3.5-turbo-0613");

			global::OpenAI.Chat.ChatResponse responses = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);
			return responses;
		}

		public async Task<string> GenerateMemoryDescription(string imageDescription)
		{
			List<global::OpenAI.Chat.Message> openAIMessages = new List<global::OpenAI.Chat.Message>();

			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"You are an AI for generating Social Media Posts"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Give the text that would be posted under the image described."));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.System, $"Only respond with the contents of the visual social media post"));
			openAIMessages.Add(new global::OpenAI.Chat.Message(global::OpenAI.Chat.Role.User, imageDescription));

			global::OpenAI.Chat.ChatRequest chatRequest = new global::OpenAI.Chat.ChatRequest(openAIMessages, "gpt-3.5-turbo-0613");

			global::OpenAI.Chat.ChatResponse responses = await openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);
			return responses;
		}

		public async Task<IReadOnlyList<string>> GenerateImage(string prompt)
		{
			ImageGenerationRequest request = new ImageGenerationRequest(
				prompt,
				Model.DallE_3,
				1, "standard", ResponseFormat.Url, "1024x1024", "natural", "Waifustasia");
			IReadOnlyList<string> result = await openAiClient.ImagesEndPoint.GenerateImageAsync(request);

			return result;
		}
	}
}
