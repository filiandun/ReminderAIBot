namespace ReminderAIBot.Models.UI.ScreenModel
{
    public class HomeScreenModel : ScreenModel
    {
        public int RemindersCount { get; set; }

        public bool HasReminders => this.RemindersCount > 0;
    }
}
