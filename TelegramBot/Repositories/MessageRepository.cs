﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Data;
using TelegramBot.Models;

namespace TelegramBot.Repositories
{
    internal class MessageRepository : IMessageRepository
    {
        public void AddMessage(string botText, string userText, long chatId, DateTime dateTime, User user)
        {
            try
            {
                using (var context = new DbTelegramContext())
                {
                    var message = new Message
                    {
                        TextBot = botText,
                        TextUser = userText,
                        UserName = user.Name,
                        ChatId = chatId,
                        dateTime = dateTime,
                        UserId = user.UserId
                    };

                    context.Messages.Add(message);
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
