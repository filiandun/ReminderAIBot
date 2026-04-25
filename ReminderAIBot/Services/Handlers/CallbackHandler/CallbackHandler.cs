using ReminderAIBot.Models.Messages;
using ReminderAIBot.Models.Callbacks;
using ReminderAIBot.Models.UI.ScreenModel;

using ReminderAIBot.Services.Messenger.SenderService;
using ReminderAIBot.Services.Messenger.ScreenRenderer;
using ReminderAIBot.Services.Callbacks.CallbackDataCodec;
using ReminderAIBot.Services.Applications.HomeApplicationService;
using ReminderAIBot.Services.Applications.ReminderApplicationService;


namespace ReminderAIBot.Services.Handlers.CallbackHandler
{
    public class CallbackHandler : ICallbackHandler
    {
        private readonly ILogger<CallbackHandler> _logger;

        private readonly IHomeApplicationService _homeApplicationService;
        private readonly IReminderApplicationService _reminderApplicationService;

        private readonly IScreenRenderer _screenRenderer;
        private readonly ISenderService _senderService;


        public CallbackHandler(ILogger<CallbackHandler> logger, IHomeApplicationService homeApplicationService, IReminderApplicationService reminderApplicationService,  IScreenRenderer messageBuilder, ISenderService senderService)
        {
            this._logger = logger;

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

            CallbackData? callbackData = CallbackDataCodec.Decode(data);

            if (callbackData is null)
            {
                this._logger.LogWarning("handle: callbackData is null");
                return;
            }

            switch (callbackData)
            {
                case NavigationCallbackData navigationCallbackData: await this.HandleNavigationAsync(navigationCallbackData, chatId, messageId); break;
                case ReminderCallbackData reminderCallbackData: await this.HandleReminderAsync(reminderCallbackData, chatId, messageId); break;
                case SettingsCallbackData settingsCallbackData: await this.HandleSettingsAsync(settingsCallbackData, chatId, messageId); break;

                default: this._logger.LogWarning($"handle: unknown type callback data: ({callbackData.GetType()})"); break;
            }
        }


        private async Task HandleNavigationAsync(NavigationCallbackData callbackData, long chatId, int messageId)
        {
            int targetPage = callbackData.TargetPage ?? 0;

            ScreenModel screenModel = callbackData.Screen switch
            {
                Screen.Home => await this._homeApplicationService.BuildHomeScreenModelAsync(chatId),

                Screen.RemindersList => await this._reminderApplicationService.BuildRemindersListScreenModelAsync(chatId, page: targetPage, pageSize: 3),

                _ => throw new Exception($"handle open screen: unknown screen {callbackData.Screen.ToString()}")
            };

            RenderedMessage? renderedMessage = this._screenRenderer.Render(screenModel);

            if (renderedMessage is null)
            {
                this._logger.LogWarning("handle open screen: rendered message is null");
                return;
            }

            await this._senderService.EditMessageAsync(chatId, messageId, renderedMessage); 
        }

        private async Task HandleReminderAsync(ReminderCallbackData callbackData, long chatId, int messageId)
        {
            switch (callbackData.Action)
            {
                case ReminderAction.Create: this._logger.LogTrace("ReminderAction.Create"); break;
                case ReminderAction.Edit: this._logger.LogTrace("ReminderAction.Edit"); break;
                case ReminderAction.Delete: this._logger.LogTrace("ReminderAction.Delete"); break;

                default: throw new Exception($"handle reminder: unknown action {callbackData.Action.ToString()}");
            };

            //RenderedMessage? renderedMessage = this._screenRenderer.Render(screenModel);

            //if (renderedMessage is null)
            //{
            //    this._logger.LogWarning("handle reminder: rendered message is null");
            //    return;
            //}

            //await this._senderService.EditMessageAsync(chatId, messageId, renderedMessage);
        }


        private async Task HandleSettingsAsync(SettingsCallbackData callbackData, long chatId, int messageId)
        {

        }
    }
}
