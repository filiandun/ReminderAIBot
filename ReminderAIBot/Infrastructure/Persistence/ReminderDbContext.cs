using Microsoft.EntityFrameworkCore;

using ReminderAIBot.Domain;


namespace ReminderAIBot.Infrastructure.Persistence
{
    public class ReminderDbContext : DbContext
    {
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<User> Users { get; set; }

        public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options)
        {

        }
    }
}
