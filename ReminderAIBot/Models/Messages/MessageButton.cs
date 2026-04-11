namespace ReminderAIBot.Models.Messages
{
    public class MessageButton
    {
        public required string Text { get; set; }
        public string? CallbackData { get; set; }
    }
}
