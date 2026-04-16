using ReminderAIBot.Models.Callbacks.Enums;


namespace ReminderAIBot.Services.Callbacks.CallbackDataBuilder
{
    public interface ICallbackDataBuilder
    {
        public string OpenScreen(UiScreen Screen);
        public string ChangePage(int page);

        public string Reminder(ReminderAction action, int reminderId);
        public string TimeZone(TimeZoneAction action, string timeZoneId);

    }
}
