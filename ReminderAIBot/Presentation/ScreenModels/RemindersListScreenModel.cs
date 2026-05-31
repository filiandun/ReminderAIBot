using ReminderAIBot.Domain;


namespace ReminderAIBot.Presentation.ScreenModels
{
    public class RemindersListScreenModel : ScreenModel
    {
        public List<Reminder> Reminders { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPrevPage => CurrentPage > 0;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
