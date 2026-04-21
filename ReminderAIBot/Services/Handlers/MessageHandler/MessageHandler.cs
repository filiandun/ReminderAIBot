using ReminderAIBot.Models.Database;
using ReminderAIBot.Models.Messages;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.ReminderParser;
using ReminderAIBot.Services.Messenger.SenderService;
using ReminderAIBot.Services.Messenger.ScreenRenderer;
using ReminderAIBot.Services.ReminderManager;


namespace ReminderAIBot.Services.Handlers.MessageHandler
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILogger<MessageHandler> _logger;

        private readonly ISenderService _senderService;

        private readonly IReminderManager _reminderService;
        private readonly IReminderParser _reminderParser;

        private readonly IScreenRenderer _screenRenderer;


        public MessageHandler(ILogger<MessageHandler> logger, ISenderService senderService, IReminderManager reminderService, IReminderParser reminderParser, IScreenRenderer messageBuilder)
        {
            this._logger = logger;

            this._senderService = senderService;

            this._reminderService = reminderService;
            this._reminderParser = reminderParser;

            this._screenRenderer = messageBuilder;           
        }


        public async Task HandleAsync(long chatId, string? messageText, string? command)
        {
            if (messageText is null)
            {
                this._logger.LogWarning("handle: messageText is null");
                return;
            }

            if (command is not null)
            {
                await this.HandleCommandAsync(chatId, command);
                return;
            }

            Reminder newReminder = await this._reminderParser.ParseAsync(messageText);
            await this._reminderService.AddReminder(chatId, newReminder);

            //RenderedMessage newReminderMessage = this._screenRenderer.NewReminderMessage(newReminder);
            //await this._senderService.SendMessageAsync(chatId, newReminderMessage);
        }


        private async Task HandleCommandAsync(long chatId, string command)
        {
            switch (command)
            {
                case "/start":
                    //await this._onboardingService.Greeting(chatId);
                    RenderedMessage? renderedMessage = this._screenRenderer.Render(new HomeScreenModel() { Text = "Home", Title = "Это главная страница"});

                    await this._senderService.SendMessageAsync(chatId, renderedMessage);

                    break;

                case "/help":
                    await this._senderService.SendMessageAsync(chatId, new RenderedMessage() { Text = "Я бот на основе Искусственного Интеллекта для создания напоминаний. Пример использования: напиши \"сегодня в 12 дня покормить кота\"" });

                    break;

                case "/list":
                    //PagedList<Reminder> reminders = new PagedList<Reminder>(await this._reminderService.GetUserReminders(chatId), 2);

                    //RenderedMessage botMessage = this._screenRenderer.RemindersList(reminders.GetPage(0).ToList(), 0);

                    //await this._senderService.SendMessageAsync(chatId, botMessage);

                    break;

                default:
                    await this._senderService.SendMessageAsync(chatId, new RenderedMessage() { Text = "К сожалению, такой команды я не знаю" });

                    break;
            }
        }
    }
}
