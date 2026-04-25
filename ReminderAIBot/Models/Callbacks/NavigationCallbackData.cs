namespace ReminderAIBot.Models.Callbacks
{
    public sealed class NavigationCallbackData : CallbackData
    {
        public Screen Screen { get; set; }
        public NavigationAction Action { get; set; }

        public int? TargetPage { get; set; }
    }
}
