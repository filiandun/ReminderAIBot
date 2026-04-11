using Telegram.Bot.Types;

namespace ReminderAIBot.Services.MessageHandler
{
    public interface IMessageHandler
    {
        public Task HandleAsync(Update update);
    }
}
