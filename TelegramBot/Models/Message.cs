using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    [Table("Message")]
    public class Message
    {
        public int MessageId { get; set; }

        [Required]
        public string? TextBot { get; set; }
        [Required]
        public string? TextUser { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public long ChatId { get; set; }
        [Required]
        public DateTime dateTime { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
