namespace ReminderAIBot.Services.Handlers.MessageHandler
{
    public interface IMessageHandler
    {
        public Task HandleAsync(long chatId, string? messageText, string? command);
    }
}
