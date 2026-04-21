using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.ReminderManager;


namespace ReminderAIBot.Services.Applications.HomeApplicationService
{
    public class HomeApplicationService : IHomeApplicationService
    {
        private readonly IReminderManager _reminderManager;


        public HomeApplicationService(IReminderManager reminderManager)
        {
            this._reminderManager = reminderManager;
        }


        public async Task<HomeScreenModel> BuildHomeScreenModelAsync(long chatId)
        {
            int reminderCount = (await this._reminderManager.GetUserReminders(chatId)).Count();

            return new HomeScreenModel()
            {
                Title = "Домашняя страница",
                Text = "",

                RemindersCount = reminderCount
            };
        }
    }
}
