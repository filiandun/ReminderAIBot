using ReminderAIBot.Models.Callbacks.Enums;


namespace ReminderAIBot.Services.Callbacks.CallbackDataBuilder
{
    public class CallbackDataBuilder : ICallbackDataBuilder
    {
        public string OpenScreen(UiScreen Screen) => $"open:{(int)Screen}";
        public string ChangePage(int page) => $"page:{page}";


        public string Reminder(ReminderAction action, int reminderId) => $"rem:{(int)action}:{reminderId}";
        public string TimeZone(TimeZoneAction action, string timeZoneId) => $"tmz:{(int)action}:{timeZoneId}";
    }
}
