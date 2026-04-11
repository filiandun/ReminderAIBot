using ReminderAIBot.Models.Database;

namespace ReminderAIBot.Services.ReminderParser
{
    public interface IReminderParser
    {
        public Task<Reminder> ParseAsync(string rawText);
    }
}
