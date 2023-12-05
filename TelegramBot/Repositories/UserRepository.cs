using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Data;
using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    internal class UserRepository : IUserRepository
    {

        public void AddUser(string name)
        {
            try
            {
                using (var context = new DbTelegramContext())
                {
                    var user = new User 
                    {
                        Name = name,
                        RoleId = 1
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public bool IsHaveUser(string userName)
        {
            using (var context = new DbTelegramContext())
            {
                var users = context.Users
                    .Where(b => b.Name.Contains(userName))
                    .ToList();
                return users.Any();
            }
        }
    }
}
