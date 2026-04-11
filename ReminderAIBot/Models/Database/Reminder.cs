namespace ReminderAIBot.Models.Database
{
    public class Reminder
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        public string RawText { get; set; }

        public string Text { get; set; }
        public DateTimeOffset? RemindAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
