namespace ReminderAIBot.Domain
{
    public class Reminder
    {
        // TODO добавить какой-нибудь status, в виде enum, который будет включать в себя условные: актуально, просрочено, отключено
        public int Id { get; set; }
        public long UserId { get; set; }

        public string RawText { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public string Text { get; set; }
        public DateTime RemindAtUtc { get; set; }
    }
}
