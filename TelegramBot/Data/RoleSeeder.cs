using Microsoft.EntityFrameworkCore;
using TelegramBot.Constants;
using TelegramBot.Models;

namespace TelegramBot.Data
{
    /// <summary>
    /// Класс для создания ролей в БД
    /// </summary>
    internal class RoleSeeder
    {
        public static void InitialRoles()
        {
            using var context = new DbTelegramContext();
            var role = new Role { RoleName = Roles.User.ToString() };
            context.Roles.Add(role);
            role = new Role { RoleName = Roles.UserSubscription.ToString() };
            context.Roles.Add(role);
            role = new Role { RoleName = Roles.Admin.ToString() };
            context.Roles.Add(role);

            context.SaveChanges();
        }

        public bool IsHaveRole(string role)
        {
            using var context = new DbTelegramContext();
            var roles = context.Roles
                .Where(b => b.RoleName.Contains(role))
                .ToList();
            return roles.Any();
        }
    }
}
