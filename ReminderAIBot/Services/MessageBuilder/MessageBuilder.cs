using System.Text;
using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.CallbackData.Enums;
using ReminderAIBot.Services.CallbackDataBuilder;
using ReminderAIBot.Models.Messages;


namespace ReminderAIBot.Services.MessageBuilder
{
    public class MessageBuilder : IMessageBuilder
    {
        private readonly ICallbackDataBuilder _callbackDataBuilder;


        public MessageBuilder(ICallbackDataBuilder callbackDataBuilder)
        {
            this._callbackDataBuilder = callbackDataBuilder;
        }


        public BotMessage NewReminderMessage(Reminder reminder)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Новое напоминание:");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"\"{reminder.Text}\"");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Напомнить");
            stringBuilder.AppendLine($"{reminder.RemindAt.Value.ToString("f")}");

            BotMessage botMessage = new BotMessage()
            {
                Text = stringBuilder.ToString(),
                Buttons = new List<MessageButton>()
                {
                    new MessageButton() { Text = "Создать", CallbackData = this._callbackDataBuilder.Reminder(CallbackReminderAction.Add, reminder.Id) },
                    new MessageButton() { Text = "Удалить", CallbackData = this._callbackDataBuilder.Reminder(CallbackReminderAction.Delete, reminder.Id) },
                    new MessageButton() { Text = "Изменить", CallbackData = this._callbackDataBuilder.Reminder(CallbackReminderAction.Edit, reminder.Id) },
                }
            };

            return botMessage;
        }

        public string RemindersList(List<Reminder> reminders)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Напоминаний: {reminders.Count}\n");

            foreach (Reminder reminder in reminders)
            {
                stringBuilder.AppendLine($"\tНапоминание: {reminder.Text}, [{reminder.CreatedAt}] [{reminder.RemindAt}]");
            }

            return stringBuilder.ToString();
        }
    }
}
