namespace TelegramBot.Repositories
{
    internal interface IUserMessageRepository
    {
        public void AddMessage(string userText, string userName);
    }
}