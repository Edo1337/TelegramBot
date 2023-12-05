using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;
using TelegramBot.Models;
using TelegramBot.OpenAI;
using TelegramBot.Repositories;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IOpenAIProxy chatOpenAI = new OpenAIProxy("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");
        var client = new TelegramBotClient("6777543810:AAF1TFUAdKjzCkIfZhpR4XYRpFT19HwB5o8");
        client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

        //RoleSeeder.InitialRoles();

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

            var user = new UserRepository();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.UtcNow} | Received a '{userMessageText}' message in chat {chatId} from @{userName}.");

            Message? botMessage = null;

            if (userMessageText.ToLower() == "/start")
            {

                if (user.IsHaveUser(userName))
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, это снова ты, на всякий случай напомню, что меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я все еще могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                }
                else
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                    user.AddUser(userName);
                    return;
                }
            }
            else
            {
                var results = await chatOpenAI.SendChatMessage(userMessageText);
                Console.WriteLine("Пишу..");
                foreach (var item in results)
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, item.Content);
                }
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
