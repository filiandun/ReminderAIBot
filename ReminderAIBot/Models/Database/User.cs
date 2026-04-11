namespace ReminderAIBot.Models.Database
{
    public record User
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }

        public TimeZoneInfo? TimeZone { get; set; }
    }
}
