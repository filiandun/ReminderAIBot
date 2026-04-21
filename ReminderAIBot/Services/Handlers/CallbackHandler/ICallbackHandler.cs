namespace ReminderAIBot.Services.Handlers.CallbackHandler
{
    public interface ICallbackHandler
    {
        public Task HandleAsync(long chatId, int messageId, string? data);
    }
}
