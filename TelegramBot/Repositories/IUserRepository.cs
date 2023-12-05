using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Repositories
{
    public interface IUserRepository
    {
        public void AddUser(string name);
        public bool IsHaveUser(string userName);
    }
}
