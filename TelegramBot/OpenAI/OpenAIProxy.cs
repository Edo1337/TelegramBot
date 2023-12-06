using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace TelegramBot.OpenAI
{
    internal class OpenAIProxy : IOpenAIProxy
    {
        readonly OpenAIClient openAIClient;

        readonly List<ChatCompletionMessage> _messages;

        public OpenAIProxy(string apiKey)
        {
            //initialize the configuration with api key and sub
            var openAIConfigurations = new OpenAIConfigurations
            {
                ApiKey = apiKey
            };

            openAIClient = new OpenAIClient(openAIConfigurations);

            _messages = new();
        }

        void StackMessages(params ChatCompletionMessage[] message)
        {
            _messages.AddRange(message);
        }
        static ChatCompletionMessage[] ToCompletionMessage(ChatCompletionChoice[] choices)
            => choices.Select(x => x.Message).ToArray();

        //Public method to Send messages to OpenAI
        public Task<ChatCompletionMessage[]> SendChatMessage(string message)
        {
            var chatMsg = new ChatCompletionMessage()
            {
                Content = message,
                Role = "user"
            };
            return SendChatMessage(chatMsg);
        }

        async Task<ChatCompletionMessage[]> SendChatMessage(ChatCompletionMessage chatMsg)
        {
            //we should send all the messages
            //so we can give Open AI context of conversation
            StackMessages(chatMsg);

            var chatCompletion = new ChatCompletion
            {
                Request = new ChatCompletionRequest
                {
                    Model = "gpt-3.5-turbo",
                    Messages = _messages.ToArray(),
                    Temperature = 0.2,
                    MaxTokens = 800
                }
            };

            try
            {
                var result = await openAIClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);
                var choices = result.Response.Choices;
                var messages = ToCompletionMessage(choices);

                //stack the response as well - everything is context to Open AI
                StackMessages(messages);

                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при отправке чата: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Дополнительная информация: {ex.InnerException.Message}");
                }
                throw new Exception("Проблема с API ChatGPT", ex);
            }

        }
    }
}
