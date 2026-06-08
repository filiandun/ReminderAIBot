using ReminderAIBot.Application.Ports;
using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Application.CommandUseCases.HomeCommandUseCases
{
    public class HomeCommandUseCases : IHomeCommandUseCases
    {
        private readonly IReminderDataStore _reminderManager;


        public HomeCommandUseCases(IReminderDataStore reminderManager)
        {
            this._reminderManager = reminderManager;
        }


        public async Task<HomeScreenModel> BuildHomeScreenModelAsync(long chatId)
        {
            int reminderCount = (await this._reminderManager.GetRemindersList(chatId)).Count();

            return new HomeScreenModel()
            {
                Title = "Домашняя страница",
                Text = "",

                RemindersCount = reminderCount
            };
        }
    }
}
