using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    [Table("UserMessage")]
    public class UserMessage
    {
        public int UserMessageId { get; set; }

        [Required]
        public string Text { get; set; }
        [Required]
        public string UserName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
