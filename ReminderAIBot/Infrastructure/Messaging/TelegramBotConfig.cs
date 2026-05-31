
namespace ReminderAIBot.Infrastructure.Messaging
{
    public class TelegramBotConfig
    {
        public required string Token { get; set; }
        public required string WebhookUrl { get; set; }
    }
}
