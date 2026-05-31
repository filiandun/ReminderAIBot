using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Application.CommandUseCases.ReminderCommandUseCases
{
    public interface IReminderCommandUseCases
    {
        public Task<RemindersListScreenModel> BuildRemindersListScreenModelAsync(long chatId, int page = 0, int pageSize = 5);
        public Task<ReminderScreenModel> BuildReminderScreenModelAsync(long chatId, int reminderId);
    }
}
