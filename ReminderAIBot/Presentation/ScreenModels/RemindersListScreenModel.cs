using ReminderAIBot.Domain;


namespace ReminderAIBot.Presentation.ScreenModels
{
    public class RemindersListScreenModel : ScreenModel
    {
        public PagedResult<Reminder> Reminders { get; set; } = new();

        public bool HasReminders => this.Reminders.ItemsCount > 0;
    }
}
