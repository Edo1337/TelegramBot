using Microsoft.EntityFrameworkCore;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Constants;
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
            var userRepos = new UserRepository();
            var messageRepos = new MessageRepository();

            var user = userRepos.FindUser(userName);
            if (user == null)
            {
                DateTime createdAt = DateTime.Now;
                userRepos.AddUser(userName, createdAt);
            }

            Telegram.Bot.Types.Message? botMessage = null;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.UtcNow} | Received a '{userMessageText}' message in chat {chatId} from @{userName}.\n");

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { Menu.Account.ToString(), Menu.Info.ToString() }
            })
            {
                ResizeKeyboard = true
            };

            if (userMessageText.ToLower() == "/start")
            {
                botMessage = await botClient.SendTextMessageAsync(chatId,
                    "Привет, меня зовут Лайт, я - телеграм-бот, созданный при помощи нейросети ChatGPT. Я могу быть твоим личным помощником " +
                    "и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, " +
                    "путешествия и многое другое.");
            }
            else if (userMessageText == Menu.Account.ToString())
            {
                botMessage = await botClient.SendTextMessageAsync(chatId, $"Аккаунт:\n\n" +
                    $"ID пользователя: {user?.UserId}" +
                    $"nПодписка: {user?.RoleId}");
            }
            else if (userMessageText == Menu.Info.ToString())
            {
                botMessage = await botClient.SendTextMessageAsync(chatId, $"");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Отправлено на сервер OpenAI..\n");

                var results = await chatOpenAI.SendChatMessage(userMessageText);
                foreach (var item in results)
                {
                    botMessage = await botClient.SendTextMessageAsync(chatId, item.Content);
                }

                if (botMessage != null)
                {
                    DateTime dateTime = DateTime.Now;
                    messageRepos.AddMessage(botMessage.Text, userMessageText, chatId, dateTime, user);
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
