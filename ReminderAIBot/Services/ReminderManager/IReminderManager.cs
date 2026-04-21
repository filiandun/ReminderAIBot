using ReminderAIBot.Models.Database;


namespace ReminderAIBot.Services.ReminderManager
{
    public interface IReminderManager
    {
        public Task<List<Reminder>> GetUserReminders(long userId);

        public Task AddReminder(long userId, Reminder reminder);
        public Task RemoveReminder(long userId, int reminderId);

    }
}
