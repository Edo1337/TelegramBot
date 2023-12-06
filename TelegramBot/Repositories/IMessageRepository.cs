using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    internal interface IMessageRepository
    {
        public void AddMessage(string botText, string userText, long chatId, DateTime dateTime, User user);
    }
}
