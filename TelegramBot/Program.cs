using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


var client = new TelegramBotClient("6777543810:AAF1TFUAdKjzCkIfZhpR4XYRpFT19HwB5o8");
client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);

var me = await client.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");

Console.ReadLine();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    if (messageText.ToLower().Contains("привет"))
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Приветик");
    }

    var userName = message.Chat.Username;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from {userName}.");

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText);
}

Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    throw new NotImplementedException();
}