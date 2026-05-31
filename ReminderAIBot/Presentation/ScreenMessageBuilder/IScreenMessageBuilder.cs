using ReminderAIBot.Presentation.Messages;
using ReminderAIBot.Presentation.ScreenModels;


namespace ReminderAIBot.Presentation.ScreenMessageBuilder
{
    public interface IScreenMessageBuilder
    {
        public BotMessage Render(ScreenModel model);
    }
}
