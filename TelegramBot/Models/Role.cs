using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    internal class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public List<User> Users { get; set; }
    }
}
