namespace ReminderAIBot.Models.UI.ScreenModel
{
    // TODO нужно добавить больше фич, типо, ближайшие напоминания и т.д.
    public class HomeScreenModel : ScreenModel
    {
        public int RemindersCount { get; set; }

        public bool HasReminders => this.RemindersCount > 0;
    }
}
