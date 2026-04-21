using ReminderAIBot.Models.Database;


namespace ReminderAIBot.Models.UI.ScreenModel
{
    public class RemindersListScreenModel : ScreenModel
    {
        public List<Reminder>? Reminders { get; set; }
        public int Page { get; set; }
        public bool HasNextPage { get; set; }
    }
}
