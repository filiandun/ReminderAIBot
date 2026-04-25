namespace ReminderAIBot.Models.Callbacks
{
    public sealed class ReminderCallbackData : CallbackData
    {
        public ReminderAction Action { get; set; }
        public int? ReminderId { get; set; }
    }
}
