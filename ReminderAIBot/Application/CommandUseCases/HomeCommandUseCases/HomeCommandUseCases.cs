using ReminderAIBot.Application.ReminderManager;

using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Application.CommandUseCases.HomeCommandUseCases
{
    public class HomeCommandUseCases : IHomeCommandUseCases
    {
        private readonly IReminderManager _reminderManager;


        public HomeCommandUseCases(IReminderManager reminderManager)
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
