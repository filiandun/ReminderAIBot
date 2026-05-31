using ReminderAIBot.Domain;


namespace ReminderAIBot.Application.Ports
{
    public interface IReminderParser
    {
        public Task<Reminder> ParseAsync(string rawText);
    }
}
