using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Data;
using TelegramBot.Models;
using TelegramBot.Constants;

namespace TelegramBot.Repositories
{
    internal class UserRepository : IUserRepository
    {

        public User AddUser(string name, DateTime createdAt)
        {
            try
            {
                using (var context = new DbTelegramContext())
                {
                    var user = new User
                    {
                        Name = name,
                        CreatedAt = createdAt,
                        RoleId = Convert.ToInt32(Roles.User)
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{createdAt} | New User: @{name}.\n");

                    return user;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public User FindUser(string name)
        {
            try
            {
                using (var context = new DbTelegramContext())
                {
                    var user = context.Users
                        .SingleOrDefault(b => b.Name == name);

                    return user;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public bool IsHaveUser(string userName)
        {
            using var context = new DbTelegramContext();
            var users = context.Users
                .Where(b => b.Name.Contains(userName))
                .ToList();
            return users.Any();
        }
    }
}
