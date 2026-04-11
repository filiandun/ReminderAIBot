using ReminderAIBot.Models.CallbackData.Enums;

namespace ReminderAIBot.Services.CallbackDataBuilder
{
    public class CallbackDataBuilder : ICallbackDataBuilder
    {
        public string Reminder(CallbackReminderAction action, int id) => $"rem:{(int)action}:{id}";
    }
}
