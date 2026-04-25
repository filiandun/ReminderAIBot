using System.Text;

using ReminderAIBot.Models.Callbacks;
using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.Messages;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.Callbacks.CallbackDataCodec;


namespace ReminderAIBot.Services.Messenger.ScreenRenderer
{
    public class ScreenRenderer : IScreenRenderer
    {
        public ScreenRenderer()
        {

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
                    new InlineButton() { Text = $"Список напоминаний [{model.RemindersCount}]", CallbackData = CallbackDataCodec.Encode(Screen.RemindersList, NavigationAction.Open) } }
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
                foreach (Reminder reminder in model.Reminders)
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

            //
            List<InlineButton> paginationButtons = new();

            if (model.HasPrevPage) paginationButtons.Add(new InlineButton() { Text = "<<", CallbackData = CallbackDataCodec.Encode(Screen.RemindersList, NavigationAction.Open, model.CurrentPage - 1) });

            paginationButtons.Add(new InlineButton() { Text = $"{model.CurrentPage + 1} из {model.TotalPages + 1}", CallbackData = "-" });

            if (model.HasNextPage) paginationButtons.Add(new InlineButton() { Text = ">>", CallbackData = CallbackDataCodec.Encode(Screen.RemindersList, NavigationAction.Open, model.CurrentPage + 1) });


            InlineButtonRow paginationButtonRow = new()
            {
                InlineButtons = paginationButtons
            };
                
            buttons.Add(paginationButtonRow);

            //
            buttons.Add(new InlineButtonRow()
            {
                InlineButtons = new List<InlineButton>()
                {
                    new InlineButton() { Text = $"Назад", CallbackData = CallbackDataCodec.Encode(Screen.Home, NavigationAction.Open) }
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
