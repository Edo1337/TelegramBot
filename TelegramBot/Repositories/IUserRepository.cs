using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    public interface IUserRepository
    {
        public User AddUser(string name, DateTime createdAt);
        public User FindUser(string name);
        public bool IsHaveUser(string userName);
    }
}
