using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.Messages;

namespace ReminderAIBot.Services.MessageBuilder
{
    public interface IMessageBuilder
    {
        public BotMessage NewReminderMessage(Reminder reminder);
        public string RemindersList(List<Reminder> reminders);
    }
}
