using ReminderAIBot.Models.UI.ScreenModel;


namespace ReminderAIBot.Services.Applications.HomeApplicationService
{
    public interface IHomeApplicationService
    {
        public Task<HomeScreenModel> BuildHomeScreenModelAsync(long chatId);
    }
}
