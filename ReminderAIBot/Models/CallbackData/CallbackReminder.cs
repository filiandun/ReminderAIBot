using ReminderAIBot.Models.CallbackData.Enums;


namespace ReminderAIBot.Models.CallbackData
{
    public class CallbackReminder
    {
        public CallbackReminderAction Action { get; set; }
        public int Id { get; set; }
    }
}
