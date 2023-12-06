using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    [Table("User")]
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? Name { get; set; }
        
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Message> Messages { get; set; }
    }
}
