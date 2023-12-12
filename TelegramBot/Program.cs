using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Constants;
using TelegramBot.Data;
using TelegramBot.OpenAI;
using TelegramBot.Repositories;

internal class Program
{
    private static async Task Main()
    {
        IOpenAIProxy chatOpenAI = new OpenAIProxy("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");
        var client = new TelegramBotClient("6777543810:AAF1TFUAdKjzCkIfZhpR4XYRpFT19HwB5o8");
        RoleSeeder.InitialRoles();

        client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

        var me = await client.GetMeAsync();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Start listening for @{me.Username}\n");

        await Task.Delay(-1);

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if (update.Message is not { } userMessage)
                return;
            if (userMessage.Text is not { } userMessageText)
                return;

            var chatId = userMessage.Chat.Id;
            var userName = userMessage.Chat.Username ?? "Anon";
            IUserRepository userRepos = new UserRepository();
            IMessageRepository messageRepos = new MessageRepository();

            var user = userRepos.FindUser(userName);
            if (user == null)
            {
                DateTime createdAt = DateTime.Now;
                userRepos.AddUser(userName, createdAt);
            }

            Message? botMessage = null;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.UtcNow} | Received a '{userMessageText}' message in chat {chatId} from @{userName}.\n");

            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] {"Account", "Подписка"}
            })
            {
                ResizeKeyboard = true
            };

            switch (userMessageText.ToLower())
            {
                case "/start":
                    {
                        botMessage = await botClient.SendTextMessageAsync(chatId,
                        "Привет, меня зовут Лайт, я - телеграм-бот для взаимодействия с ChatGPT. Я могу быть твоим личным помощником " +
                        "и дать тебе различные советы в различных областях, таких как программирование, математика, здоровье, фитнес, кулинария, " +
                        "путешествия и многое другое.", cancellationToken: token);
                        break;
                    }
                case "account":
                    {
                        botMessage = await botClient.SendTextMessageAsync(chatId,
                        $"Аккаунт:\n\n" +
                        $"ID пользователя: {user?.UserId}\n" +
                        $"Подписка: {user?.RoleId}",
                        cancellationToken: token);
                        break;
                    }
                case "info":
                    {
                        botMessage = await botClient.SendTextMessageAsync(chatId,
                        $"",
                        cancellationToken: token);
                        break;
                    }
                case "подписка":
                    {
                        IEnumerable<LabeledPrice> prices = new List<LabeledPrice>() { new LabeledPrice("Подписка на месяц", 20000) };
                        botMessage = await botClient.SendInvoiceAsync(chatId,
                        "Оплата", "Данная подписка предоставляет доступ к неограниченному количеству запросов", "1",
                        "1744374395:TEST:2a04d2bebf1431349a1f", "RUB", prices);
                        break;
                    }
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Отправлено на сервер OpenAI..\n");

                        var results = await chatOpenAI.SendChatMessage(userMessageText);
                        foreach (var item in results)
                        {
                            botMessage = await botClient.SendTextMessageAsync(chatId, item.Content, cancellationToken: token);
                        }

                        if (botMessage != null)
                        {
                            DateTime dateTime = DateTime.Now;
                            messageRepos.AddMessage(botMessage.Text, userMessageText, chatId, dateTime, user);
                        }

                        Console.WriteLine($"{DateTime.UtcNow} | Sented a '{botMessage?.Text}' message in chat {chatId} to @{userName}.\n");
                        break;
                    }
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new Exception();
        }
    }

}
