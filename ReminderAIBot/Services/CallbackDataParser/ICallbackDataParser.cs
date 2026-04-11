using ReminderAIBot.Models.CallbackData;

namespace ReminderAIBot.Services.CallbackDataParser
{
    public interface ICallbackDataParser
    {
        public CallbackReminder? ParseReminder(string callbackData);
    }
}
