using System.Text.Json.Serialization;

namespace ReminderAIBot.Models
{
    public class ReminderDraft
    {
        [JsonPropertyName("Text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("RemindAt")]
        public DateTimeOffset RemindAt { get; set; }

        [JsonPropertyName("IsValid")]
        public bool IsValid { get; set; }
    }
}
