using ReminderAIBot.Models.UI.ScreenModel;


namespace ReminderAIBot.Services.Applications.ReminderApplicationService
{
    public interface IReminderApplicationService
    {
        public Task<RemindersListScreenModel> BuildRemindersListScreenModelAsync(long chatId, int page = 0, int pageSize = 5);
    }
}
