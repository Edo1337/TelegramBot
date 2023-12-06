using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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
        RoleSeeder.InitialRoles();

        client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

        var me = await client.GetMeAsync();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Start listening for @{me.Username}\n");

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
            var message = new MessageRepository();

            bool isHaveUser = user.IsHaveUser(userName);

            Telegram.Bot.Types.Message? botMessage = null;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.UtcNow} | Received a '{userMessageText}' message in chat {chatId} from @{userName}.\n");

//            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
//{
//    new KeyboardButton[] { "Инфо", "" },
//    new KeyboardButton[] { "Help me", "Call me ☎️" },
//})
//            {
//                ResizeKeyboard = true
//            };

//            Telegram.Bot.Types.Message sentMessage = await botClient.SendTextMessageAsync(
//                chatId: chatId,
//                text: "Choose a response",
//                replyMarkup: replyKeyboardMarkup);

            if (userMessageText.ToLower() == "/start")
            {
                if (isHaveUser)
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, это снова ты, на всякий случай напомню, что меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я все еще могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                }
                else
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, "Привет, меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я могу быть твоим личным помощником и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, путешествия и многое другое.");
                    DateTime createdAt = DateTime.Now;
                    user.AddUser(userName, createdAt);
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Думаю..\n");
                var results = await chatOpenAI.SendChatMessage(userMessageText);
                foreach (var item in results)
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, item.Content);
                }

                if (botMessage != null)
                {
                    DateTime dateTime = DateTime.Now;
                    message.AddMessage(botMessage.Text, userMessageText, chatId, dateTime, user.FindUser(userName));
                }

                Console.WriteLine($"{DateTime.UtcNow} | Sented a '{botMessage?.Text}' message in chat {chatId} to @{userName}.\n");
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new Exception();
        }
    }

}
