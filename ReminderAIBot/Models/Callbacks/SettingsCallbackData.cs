namespace ReminderAIBot.Models.Callbacks
{
    public class SettingsCallbackData : CallbackData
    {
        public SettingsAction Action {  get; set; }

        public string? TimeZoneId { get; set; }
    }
}
