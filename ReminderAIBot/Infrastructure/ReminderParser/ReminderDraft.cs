using System.Text.Json.Serialization;


namespace ReminderAIBot.Infrastructure.ReminderParser
{
    public class ReminderDraft
    {
        [JsonPropertyName("Text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("RemindAtUtc")]
        public DateTime RemindAtUtc { get; set; }

        [JsonPropertyName("IsValid")]
        public bool IsValid { get; set; }
    }
}
