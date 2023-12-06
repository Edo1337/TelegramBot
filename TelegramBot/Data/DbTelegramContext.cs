using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Data
{

    public class DbTelegramContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=GPTLite_DB;Trusted_Connection=True");
        }
    }
}