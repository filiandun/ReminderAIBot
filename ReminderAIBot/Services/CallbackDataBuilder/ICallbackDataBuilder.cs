using ReminderAIBot.Models.CallbackData.Enums;

namespace ReminderAIBot.Services.CallbackDataBuilder
{
    public interface ICallbackDataBuilder
    {
        public string Reminder(CallbackReminderAction action, int id);
    }
}
