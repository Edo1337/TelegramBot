using TelegramBot.Constants;
using TelegramBot.Models;

namespace TelegramBot.Data
{
    internal class RoleSeeder
    {
        public static void InitialRoles()
        {
            using (var context = new DbTelegramContext())
            {
                var role = new Role { RoleName = Roles.User.ToString() };
                    context.Roles.Add(role);
                role = new Role { RoleName = Roles.UserSubscription.ToString() };
                    context.Roles.Add(role);
                role = new Role { RoleName = Roles.Admin.ToString() };
                    context.Roles.Add(role);

                context.SaveChanges();
            }
        }

    }
}
