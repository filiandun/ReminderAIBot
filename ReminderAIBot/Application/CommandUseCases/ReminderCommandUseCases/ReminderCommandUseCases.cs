using ReminderAIBot.Domain;

using ReminderAIBot.Application.Ports;

using ReminderAIBot.Presentation;
using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Application.CommandUseCases.ReminderCommandUseCases
{
    public class ReminderCommandUseCases : IReminderCommandUseCases
    {
        private readonly ILogger<ReminderCommandUseCases> _logger;

        private readonly IReminderDataStore _reminderManager;


        public ReminderCommandUseCases(ILogger<ReminderCommandUseCases> logger, IReminderDataStore reminderManager)
        {
            this._logger = logger;
            this._reminderManager = reminderManager;
        }


        public async Task<RemindersListScreenModel> BuildRemindersListScreenModelAsync(long chatId, int currentPage = 0, int pageSize = 5)
        {
            List<Reminder> reminders = (await this._reminderManager.GetRemindersList(chatId)).Skip(currentPage * pageSize).Take(pageSize).ToList();

            int remindersCount = (await this._reminderManager.GetRemindersList(chatId)).Count;
            int totalPages = (int)Math.Ceiling((double)remindersCount / pageSize);

            return new RemindersListScreenModel()
            {
                Title = "Список ваших напоминаний",
                Text = $"всего напоминаний: {remindersCount}",

                Reminders = new PagedResult<Reminder>(reminders, currentPage, totalPages),
            };
        }


        public async Task<ReminderScreenModel> BuildReminderScreenModelAsync(long chatId, int reminderId)
        {
            Reminder reminder = (await this._reminderManager.GetReminder(chatId, reminderId));

            return new ReminderScreenModel()
            {
                ReminderId = reminder.Id,

                Title = "Просмотр напоминания",
                Text = "тут вы можете управлять напоминанием",

                IsEnabled = true,

                ReminderText = reminder.Text,
                RawText = reminder.RawText,

                RemindAt = reminder.RemindAtUtc.ToString("U"),
                CreatedAt = reminder.CreatedAtUtc.ToString("U"),
            };
        }
    }
}
