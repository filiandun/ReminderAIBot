using Telegram.Bot.Types;

namespace ReminderAIBot.Services.Handlers.UpdateHandler
{
    public interface IUpdateHandler
    {
        public Task HandleAsync(Update update);
    }
}
