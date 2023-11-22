using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace TelegramBot.OpenAI
{
    internal interface IOpenAIProxy
    {
        Task<ChatCompletionMessage[]> SendChatMessage(string message);
    }
}
