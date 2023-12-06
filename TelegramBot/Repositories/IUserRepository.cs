using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    public interface IUserRepository
    {
        public void AddUser(string name);
        public User FindUser(string name);
        public bool IsHaveUser(string userName);
    }
}
