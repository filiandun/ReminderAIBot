using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ReminderAIBot.Models.Database;
using ReminderAIBot.Services.SenderService;
using ReminderAIBot.Services.MessageBuilder;
using ReminderAIBot.Services.ReminderParser;
using ReminderAIBot.Services.ReminderService;
using ReminderAIBot.Services.OnboardingService;
using ReminderAIBot.Services.CallbackDataParser;
using ReminderAIBot.Services.CallbackDataBuilder;
using ReminderAIBot.Models.CallbackData.Enums;
using ReminderAIBot.Models.CallbackData;
using ReminderAIBot.Models.Messages;


namespace ReminderAIBot.Services.MessageHandler
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILogger<MessageHandler> _logger;

        private readonly IReminderParser _reminderParser;
        private readonly IReminderService _reminderService;

        private readonly ISenderService _senderService;

        private readonly IMessageBuilder _messageBuilder;

        private readonly IOnboardingService _onboardingService;

        private readonly ICallbackDataBuilder _callbackDataBuilder;
        private readonly ICallbackDataParser _callbackDataParser;


        public MessageHandler(ILogger<MessageHandler> logger, IReminderParser reminderParser, IReminderService reminderService, ISenderService senderService, IMessageBuilder messageBuilder, IOnboardingService onboardingService, ICallbackDataBuilder callbackDataBuilder, ICallbackDataParser callbackDataParser)
        {
            this._logger = logger;

            this._reminderParser = reminderParser;
            this._reminderService = reminderService;

            this._senderService = senderService;

            this._messageBuilder = messageBuilder;

            this._onboardingService = onboardingService;

            this._callbackDataBuilder = callbackDataBuilder;
            this._callbackDataParser = callbackDataParser;
        }


        public async Task HandleAsync(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message: await this.HandleMessageAsync(update); break;
                case UpdateType.CallbackQuery: await this.HandleCallbackAsync(update); break;

                case UpdateType.ChatMember: break;
                case UpdateType.EditedMessage: break;
                case UpdateType.Unknown: break;

                default: this._logger.LogWarning("Unhandled update type: {Type}", update.Type); break;
            }
        }


        private async Task HandleMessageAsync(Update update)
        {
            if (update.Message?.Text is null)
            {
                this._logger.LogWarning("update.Message.Text is null");
                return;
            }

            //
            MessageEntity? commandEntity = update.Message.Entities?.FirstOrDefault(e => e.Type == MessageEntityType.BotCommand) ?? null;
            if (commandEntity is not null)
            {
                string command = update.Message.Text.Substring(commandEntity.Offset, commandEntity.Length);
                await this.HandleCommandAsync(update.Message.Chat.Id, command);

                return;
            }

            //
            string rawText = update.Message.Text;
            Reminder newReminder = await this._reminderParser.ParseAsync(rawText);

            await this._reminderService.AddReminder(update.Message.Chat.Id, newReminder);

            BotMessage newReminderMessage = this._messageBuilder.NewReminderMessage(newReminder);

            await this._senderService.SendMessageAsync(update.Message.Chat.Id, newReminderMessage);
        }

        private async Task HandleCommandAsync(long chatId, string command)
        {
            switch (command)
            {
                case "/start":
                    await this._onboardingService.Greeting(chatId);
                    
                    break;

                case "/help": 
                    await this._senderService.SendMessageAsync(chatId, new BotMessage() { Text = "Я бот на основе Искусственного Интеллекта для создания напоминаний. Пример использования: напиши \"сегодня в 12 дня покормить кота\"" }); 
                    
                    break;

                case "/list":
                    List<Reminder> reminders = await this._reminderService.GetUserReminders(chatId);
                    string remindersMessage = this._messageBuilder.RemindersList(reminders);

                    await this._senderService.SendMessageAsync(chatId, new BotMessage() { Text = remindersMessage });

                    break;

                default: 
                    await this._senderService.SendMessageAsync(chatId, new BotMessage() { Text = "К сожалению, такой команды я не знаю" }); 
                    
                    break;
            }
        }

        /* TODO сделать нормальные проверки на null у callback.Message.MessageId и update.Message?.Chat.Id
         * может правильнее даже передавать сюда уже проверенный объект message, который проверяться будет в основном handle */
        private async Task HandleCallbackAsync(Update update)
        {
            var callback = update.CallbackQuery;
            if (callback?.Data is null) return;

            long chatId = update.CallbackQuery?.Message?.Chat.Id ?? throw new Exception("handle callback: chatId is null");

            CallbackReminder? callbackReminder = this._callbackDataParser.ParseReminder(callback.Data);

            if (callbackReminder is null) throw new Exception("handle callback: callbackReminder is null");

            switch (callbackReminder.Action)
            {
                case CallbackReminderAction.Add:
                    await this._senderService.SendMessageAsync(chatId, new BotMessage() { Text = "Напоминание успешно создано!" }); 
                    //await this._senderService.EditMessageAsync(chatId, callback.Message.MessageId, new BotMessage() { Text = "Напоминание создано!" }); 

                    break;

                case CallbackReminderAction.Delete:
                    await this._reminderService.RemoveReminder(chatId, callbackReminder.Id);

                    await this._senderService.SendMessageAsync(chatId, new BotMessage() { Text = "Напоминание удалено." });
                    //await this._senderService.EditMessageAsync(chatId, callback.Message.MessageId, new BotMessage() { Text = "Напоминание удалено" }); 

                    break;

                case CallbackReminderAction.Edit: break;

                default: this._logger.LogWarning($"Unknown callback: {callback?.Data}"); break;
            }
        }


        private bool IsCommand(Message message, out string command)
        {
            MessageEntity? commandEntity = message.Entities?.FirstOrDefault(e => e.Type == MessageEntityType.BotCommand) ?? null;
            if (commandEntity is not null)
            {
                command = message.Text.Substring(commandEntity.Offset, commandEntity.Length);

                return true;
            }

            command = "";
            return false;
        }
    }
}