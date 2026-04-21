using ReminderAIBot.Models.Messages;
using ReminderAIBot.Models.Callbacks;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.Messenger.SenderService;
using ReminderAIBot.Services.Messenger.ScreenRenderer;
using ReminderAIBot.Services.Callbacks.CallbackDataParser;
using ReminderAIBot.Services.Applications.HomeApplicationService;
using ReminderAIBot.Services.Applications.ReminderApplicationService;


namespace ReminderAIBot.Services.Handlers.CallbackHandler
{
    public class CallbackHandler : ICallbackHandler
    {
        private readonly ILogger<CallbackHandler> _logger;

        private readonly ICallbackDataParser _callbackDataParser;

        private readonly IHomeApplicationService _homeApplicationService;
        private readonly IReminderApplicationService _reminderApplicationService;

        private readonly IScreenRenderer _screenRenderer;
        private readonly ISenderService _senderService;


        public CallbackHandler(ILogger<CallbackHandler> logger, ICallbackDataParser callbackDataParser, IHomeApplicationService homeApplicationService, IReminderApplicationService reminderApplicationService,  IScreenRenderer messageBuilder, ISenderService senderService)
        {
            this._logger = logger;

            this._callbackDataParser = callbackDataParser;

            this._homeApplicationService = homeApplicationService;
            this._reminderApplicationService = reminderApplicationService;

            this._senderService = senderService;
            this._screenRenderer = messageBuilder;
        }


        public async Task HandleAsync(long chatId, int messageId, string? data)
        {
            if (data is null)
            {
                this._logger.LogWarning("handle: callback data is null");
                return;
            }

            CallbackData? callbackData = this._callbackDataParser.Parse((string)data);

            if (callbackData is null)
            {
                this._logger.LogWarning("handle: callbackData is null");
                return;
            }

            switch (callbackData.Action)
            {
                case ScreenAction.Open: await this.HandleOpenScreenAsync(callbackData.Screen, chatId, messageId); break;

                //case ScreenAction.NextPage: await this.HandleChangePageAsync(callbackData.Screen, chatId, messageId); break;
                //case ScreenAction.PrevPage: await this.HandleChangePageAsync(callbackData.Screen, chatId, messageId); break;

                default: this._logger.LogWarning($"handle: unknown type callback data: ({callbackData.Action})"); break;
            }
        }


        private async Task HandleOpenScreenAsync(Screen screen, long chatId, int messageId)
        {
            ScreenModel screenModel = screen switch
            {
                Screen.Home => await this._homeApplicationService.BuildHomeScreenModelAsync(chatId),
                Screen.RemindersList => await this._reminderApplicationService.BuildRemindersListScreenModelAsync(chatId),
                _ => throw new Exception($"handle open screen: unknown screen {screen.ToString()}")
            };

            RenderedMessage? renderedMessage = this._screenRenderer.Render(screenModel);

            if (renderedMessage is null)
            {
                this._logger.LogWarning("handle open screen: rendered message is null");
                return;
            }

            await this._senderService.EditMessageAsync(chatId, messageId, renderedMessage); 
        }

    }
}
