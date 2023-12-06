using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Data;
using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    internal class UserMessageRepository
    {

        public void AddMessage(string userText, User user)
        {
            try
            {
                using (var context = new DbTelegramContext())
                {
                    var message = new UserMessage
                    {
                        Text = userText,
                        UserName = user.Name,
                        UserId = user.UserId
                    };

                    context.UserMessages.Add(message);
                    context.SaveChanges();
                }
            }
            catch
            {
                throw new Exception();
            }
        }

    }
}
