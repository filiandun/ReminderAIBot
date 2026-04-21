using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.ReminderManager;


namespace ReminderAIBot.Services.Applications.ReminderApplicationService
{
    public class ReminderApplicationService : IReminderApplicationService
    {
        private readonly ILogger<ReminderApplicationService> _logger;

        private readonly IReminderManager _reminderManager;


        public ReminderApplicationService(ILogger<ReminderApplicationService> logger, IReminderManager reminderManager)
        {
            this._logger = logger;
            this._reminderManager = reminderManager;
        }


        public async Task<RemindersListScreenModel> BuildRemindersListScreenModelAsync(long chatId, int page = 0, int pageSize = 5)
        {
            List<Reminder> reminders = (await this._reminderManager.GetUserReminders(chatId)).Skip(page * pageSize).Take(pageSize).ToList();

            return new RemindersListScreenModel()
            {
                Title = "Список ваших напоминаний",
                Text = $"всего напоминаний: {reminders.Count}",

                Reminders = reminders,
                
                Page = page,
                HasNextPage = true
            };
        }
    }
}
