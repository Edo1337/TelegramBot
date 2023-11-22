using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var client = new TelegramBotClient("6777543810:AAF1TFUAdKjzCkIfZhpR4XYRpFT19HwB5o8");
        client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

        var me = await client.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");

        Console.ReadLine();

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Message? botMessage = null;

            if (messageText.ToLower() == "/start")
            {
                botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                return;
            }

            var userName = message.Chat.Username ?? "Anon";

            Console.WriteLine($"{DateTime.UtcNow} | Received a '{messageText}' message in chat {chatId} from @{userName}.");

            if (botMessage != null)
                Console.WriteLine($"{DateTime.UtcNow} | Sented a '{botMessage?.Text}' message in chat {chatId}.");
        }

        Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
