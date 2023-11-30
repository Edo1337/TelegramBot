using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.OpenAI;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IOpenAIProxy chatOpenAI = new OpenAIProxy("API Key");
        var client = new TelegramBotClient("6777543810:AAF1TFUAdKjzCkIfZhpR4XYRpFT19HwB5o8");
        client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

        var me = await client.GetMeAsync();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Start listening for @{me.Username}");

        Console.ReadLine();

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message is not { } userMessage)
                return;
            if (userMessage.Text is not { } userMessageText)
                return;

            var chatId = userMessage.Chat.Id;
            var userName = userMessage.Chat.Username ?? "Anon";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.UtcNow} | Received a '{userMessageText}' message in chat {chatId} from @{userName}.");

            var results = await chatOpenAI.SendChatMessage(userMessageText);

            Message? botMessage = null;

            if (userMessageText.ToLower() == "/start")
            {
                botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                return;
            }

            foreach (var item in results)
            {
                botMessage = await botClient.SendTextMessageAsync(chatId, item.Content);
            }

            if (botMessage != null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{DateTime.UtcNow} | Sented a '{botMessage?.Text}' message in chat {chatId} to @{userName}.");
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new Exception();
        }
    }

}
