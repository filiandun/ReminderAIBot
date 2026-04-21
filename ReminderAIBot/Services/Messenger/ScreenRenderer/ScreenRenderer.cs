using System.Text;

using ReminderAIBot.Models.Callbacks;
using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.Messages;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.Callbacks.CallbackDataBuilder;


namespace ReminderAIBot.Services.Messenger.ScreenRenderer
{
    public class ScreenRenderer : IScreenRenderer
    {
        private readonly ICallbackDataBuilder _callbackDataBuilder;


        public ScreenRenderer(ICallbackDataBuilder callbackDataBuilder)
        {
            this._callbackDataBuilder = callbackDataBuilder;
        }


        public RenderedMessage? Render(ScreenModel model)
        {
            RenderedMessage? botMessage = null;

            switch (model)
            {
                case HomeScreenModel homeScreenModel: botMessage = this.RenderHome(homeScreenModel); break;

                case RemindersListScreenModel remindersListScreenModel: botMessage = this.RenderRemindersList(remindersListScreenModel); break;
            }

            return botMessage;
        }

        private RenderedMessage RenderHome(HomeScreenModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(model.Title);
            stringBuilder.AppendLine(model.Text);


            List<InlineButtonRow> buttons = new List<InlineButtonRow>()
            {
                new InlineButtonRow() { InlineButtons = new List<InlineButton>()
                {
                    new InlineButton() { Text = $"Список напоминаний [{model.RemindersCount}]", CallbackData = this._callbackDataBuilder.Build(Screen.RemindersList, ScreenAction.Open) } }
                },
            };


            RenderedMessage botMessage = new RenderedMessage()
            {
                Text = stringBuilder.ToString(),
                InlineButtonRows = buttons
            };

            return botMessage;
        }

        private RenderedMessage RenderRemindersList(RemindersListScreenModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(model.Title);
            stringBuilder.AppendLine(model.Text);


            List<InlineButtonRow> buttons = new();

            if (model.Reminders is not null)
            {
                foreach (Reminder reminder in model.Reminders.GetPage(1))
                {
                    buttons.Add
                    (
                        new InlineButtonRow() 
                        { 
                            InlineButtons = new List<InlineButton>() 
                            { 
                                new InlineButton() { Text = $"[{reminder.RemindAt.Value.DateTime.ToString("g")}]{reminder.Text}" } 
                            }
                        }
                    );
                }
            }

            buttons.Add(new InlineButtonRow()
            {
                InlineButtons = new List<InlineButton>()
                {
                    new InlineButton() { Text = $"Назад", CallbackData = this._callbackDataBuilder.Build(Screen.Home, ScreenAction.Open) }
                }
            });

            RenderedMessage botMessage = new RenderedMessage()
            {
                Text = stringBuilder.ToString(),
                InlineButtonRows = buttons
            };

            return botMessage;
        }
    }
}
