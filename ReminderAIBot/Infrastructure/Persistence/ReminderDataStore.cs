using ReminderAIBot.Domain;
using ReminderAIBot.Application.Ports;

using Microsoft.EntityFrameworkCore;


namespace ReminderAIBot.Infrastructure.Persistence
{
    public class ReminderDataStore : IReminderDataStore
    {
        private readonly ILogger<ReminderDataStore> _logger;

        private readonly IDbContextFactory<ReminderDbContext> _dbContextFactory;


        public ReminderDataStore(ILogger<ReminderDataStore> logger, IDbContextFactory<ReminderDbContext> dbContextFactory)
        {
            this._logger = logger;

            this._dbContextFactory = dbContextFactory;
        }


        public async Task<Reminder> GetReminder(long platformUserId, int reminderId)
        {
            using var database = await this._dbContextFactory.CreateDbContextAsync();

            User user = database.Users.FirstOrDefault(u => u.PlatformUserId == platformUserId) ?? throw new Exception($"get user reminder: user by platform id [{platformUserId}] not found ");
            
            this._logger.LogTrace($"user {platformUserId} get reminder [{reminderId}]");

            Reminder reminder = database.Reminders.FirstOrDefault(r => r.Id == reminderId) ?? throw new Exception($"get user reminder: reminder [{reminderId}] not found");
            if (reminder.UserId != user.Id) throw new Exception($"get user reminder: user [{user.Id}] try get reminder [{platformUserId}] other user");

            return reminder;
        }


        public async Task<List<Reminder>> GetRemindersList(long platformUserId)
        {
            using var database = await this._dbContextFactory.CreateDbContextAsync();

            User? user = database.Users.FirstOrDefault(u => u.PlatformUserId == platformUserId);
            if (user is null) return new List<Reminder>();

            this._logger.LogTrace($"user {platformUserId} get all reminders");

            List<Reminder> remindersList = database.Reminders.Where(r => r.UserId == user.Id).ToList();

            return remindersList;
        }

        // TODO думаю, что тут нужно принимать reminderDraft, а дальше уже создавать тут объект reminder
        public async Task AddReminder(long platformUserId, Reminder reminder)
        {
            using var database = await this._dbContextFactory.CreateDbContextAsync();

            User? user = database.Users.FirstOrDefault(u => u.PlatformUserId == platformUserId);
            if (user is null)
            {
                user = new User { Id = new Random().Next(), PlatformUserId = platformUserId, TimeZoneId = TimeZoneInfo.Local.ToString() };
                await database.Users.AddAsync(user);
            }

            reminder.UserId = user.Id;

            this._logger.LogTrace($"add: user [{platformUserId}] add new reminder {reminder.Text}");

            await database.Reminders.AddAsync(reminder);

            await database.SaveChangesAsync(); // TODO temp
        }

        public async Task RemoveReminder(long platformUserId, int reminderId)
        {
            using var database = await this._dbContextFactory.CreateDbContextAsync();

            User user = database.Users.FirstOrDefault(u => u.PlatformUserId == platformUserId) ?? throw new Exception($"remove reminder: user by platform id [{platformUserId}] not found");
            
            if (user.PlatformUserId != platformUserId) throw new Exception("try remove reminder by other user");

            this._logger.LogTrace($"remove reminder: user [{platformUserId}] remove reminder [{reminderId}]");

            Reminder reminder = database.Reminders.FirstOrDefault(r => r.Id == reminderId) ?? throw new Exception($"remove reminder: reminder by id [{reminderId}] not found");
            if (reminder.UserId != user.Id) throw new Exception($"remove reminder: user [{user.Id}] try remove reminder [{platformUserId}] other user");

            database.Reminders.Remove(reminder);

            await database.SaveChangesAsync(); // TODO temp
        }
    }
}
