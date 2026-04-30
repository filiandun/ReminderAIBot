using ReminderAIBot.Models.Database;


namespace ReminderAIBot.Services.ReminderManager
{
    public interface IReminderManager
    {
        public Task<Reminder> GetReminder(long platformUserId, int reminderId);
        public Task<List<Reminder>> GetRemindersList(long platfomrUserId);

        public Task AddReminder(long userId, Reminder reminder);
        public Task RemoveReminder(long userId, int reminderId);

    }
}
