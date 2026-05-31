using Telegram.Bot.Types;


namespace ReminderAIBot.Presentation.Handlers.UpdateHandler
{
    public interface IUpdateHandler
    {
        public Task HandleAsync(Update update);
    }
}
