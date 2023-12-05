using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    [Table("Role")]
    internal class Role
    {
        public int RoleId { get; set; }

        [Required]
        [MaxLength(25)]
        public string? RoleName { get; set; }

        public List<User> Users { get; set; }
    }
}
