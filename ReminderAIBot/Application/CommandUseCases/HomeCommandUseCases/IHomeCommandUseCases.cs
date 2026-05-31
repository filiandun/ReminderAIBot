using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Application.CommandUseCases.HomeCommandUseCases
{
    public interface IHomeCommandUseCases
    {
        public Task<HomeScreenModel> BuildHomeScreenModelAsync(long chatId);
    }
}
