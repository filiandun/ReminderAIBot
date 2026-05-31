using ReminderAIBot.Presentation.Messages;


namespace ReminderAIBot.Infrastructure.Messaging.SenderService
{
    public interface ISenderService
    {
        public Task SendMessageAsync(long chatId, BotMessage message);

        public Task EditMessageAsync(long chatId, int messageId, BotMessage message);

        public Task DeleteMessageAsync(long chatId, int messageId);

        public Task AnswerCallbackQuery(string callbackQueryId, string text);
    }
}
