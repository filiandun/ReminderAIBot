using System.Text;

using ReminderAIBot.Domain;

using ReminderAIBot.Presentation.Messages;
using ReminderAIBot.Presentation.Callbacks;
using ReminderAIBot.Presentation.ScreenModels;
using ReminderAIBot.Presentation.Callbacks.CallbackCommands.Home;
using ReminderAIBot.Presentation.Callbacks.CallbackCommands.Reminder;


namespace ReminderAIBot.Presentation.ScreenMessageBuilder
{
    public class ScreenMessageBuilder : IScreenMessageBuilder
    {
        public ScreenMessageBuilder()
        {

        }


        public BotMessage Render(ScreenModel model)
        {
            return model switch
            {
                HomeScreenModel homeScreenModel => this.RenderHome(homeScreenModel),
                RemindersListScreenModel remindersListScreenModel => this.RenderRemindersList(remindersListScreenModel),
                ReminderScreenModel reminderScreenModel => this.RenderReminder(reminderScreenModel),

                _ => throw new NotSupportedException($"render: unsupported screen model type: {model.GetType().Name}")
            };
        }

        private BotMessage RenderHome(HomeScreenModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(model.Title);
            stringBuilder.AppendLine(model.Text);


            List<InlineButtonRow> buttons = new List<InlineButtonRow>()
            {
                new InlineButtonRow() 
                { 
                    InlineButtons = new List<InlineButton>()
                    {
                        new InlineButton() { Text = $"Список напоминаний [{model.RemindersCount}]", CallbackData = CallbackDataCodec.Encode(new OpenRemindersListCommand(0)) } 
                    }
                },
            };


            BotMessage botMessage = new BotMessage()
            {
                Text = stringBuilder.ToString(),
                InlineButtonRows = buttons
            };

            return botMessage;
        }

        private BotMessage RenderRemindersList(RemindersListScreenModel model)
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
                                new InlineButton() { Text = $"[{reminder.RemindAtUtc.ToString("g")}]{reminder.Text}", CallbackData = CallbackDataCodec.Encode(new OpenReminderDetailsCommand(reminder.Id)) }
                            }
                        }
                    );
                }
            }

            //
            List<InlineButton> paginationButtons = new();

            if (model.HasPrevPage) paginationButtons.Add(new InlineButton() { Text = "<<", CallbackData = CallbackDataCodec.Encode(new OpenRemindersListCommand(model.CurrentPage - 1)) });

            paginationButtons.Add(new InlineButton() { Text = $"{model.CurrentPage + 1} из {model.TotalPages + 1}", CallbackData = "-" });

            if (model.HasNextPage) paginationButtons.Add(new InlineButton() { Text = ">>", CallbackData = CallbackDataCodec.Encode(new OpenRemindersListCommand(model.CurrentPage + 1)) });


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
                    new InlineButton() { Text = $"Назад", CallbackData = CallbackDataCodec.Encode(new OpenHomeCommand()) }
                }
            });

            BotMessage botMessage = new BotMessage()
            {
                Text = stringBuilder.ToString(),
                InlineButtonRows = buttons
            };

            return botMessage;
        }

        private BotMessage RenderReminder(ReminderScreenModel model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(model.Title);
            stringBuilder.AppendLine(model.Text);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"\"{model.ReminderText}\"");
            stringBuilder.AppendLine($"Напомнить: {model.RemindAt}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Создано: {model.CreatedAt}");
            stringBuilder.AppendLine($"Изначальный запрос: {model.RawText}");


            List<InlineButtonRow> buttons = new List<InlineButtonRow>()
            {
                new InlineButtonRow() 
                { 
                    InlineButtons = new List<InlineButton>() 
                    {
                        new InlineButton() { Text = $"Редактировать", CallbackData = CallbackDataCodec.Encode(new EditReminderCommand(model.ReminderId)) },
                        new InlineButton() { Text = $"Удалить", CallbackData = CallbackDataCodec.Encode(new DeleteReminderCommand(model.ReminderId)) },
                        new InlineButton() { Text = $"Выключить", CallbackData = "-"},
                    },
                },
            };

            //
            buttons.Add(new InlineButtonRow()
            {
                InlineButtons = new List<InlineButton>()
                {
                    new InlineButton() { Text = $"Назад", CallbackData = CallbackDataCodec.Encode(new OpenRemindersListCommand(0)) }
                }
            });

            BotMessage botMessage = new BotMessage()
            {
                Text = stringBuilder.ToString(),
                InlineButtonRows = buttons
            };

            return botMessage;
        }
    }
}
