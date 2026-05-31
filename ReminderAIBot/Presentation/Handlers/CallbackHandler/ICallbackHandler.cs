
namespace ReminderAIBot.Presentation.Handlers.CallbackHandler
{
    public interface ICallbackHandler
    {
        public Task HandleAsync(long chatId, int messageId, string callbackQueryId, string? data);
    }
}
