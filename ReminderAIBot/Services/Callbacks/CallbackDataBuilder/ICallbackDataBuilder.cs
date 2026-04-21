using ReminderAIBot.Models.Callbacks;


namespace ReminderAIBot.Services.Callbacks.CallbackDataBuilder
{
    public interface ICallbackDataBuilder
    {
        public string Build(Screen screen, ScreenAction action);
    }
}
