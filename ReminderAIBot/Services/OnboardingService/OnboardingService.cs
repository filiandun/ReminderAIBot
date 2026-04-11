using ReminderAIBot.Models;
using ReminderAIBot.Services.SenderService;


namespace ReminderAIBot.Services.OnboardingService
{
    public class OnboardingService : IOnboardingService
    {
        private readonly ILogger<OnboardingService> _logger;

        private readonly ISenderService _senderService;


        public OnboardingService(ILogger<OnboardingService> logger, ISenderService senderService)
        {
            this._logger = logger;

            this._senderService = senderService;
        }


        public async Task Greeting(long chatId)
        {
            await this._senderService.SendMessageAsync(chatId, new BotMessage { Text = "Привет!\nЯ бот для создания напоминаний. Внутри себя я использую ИИ для лучшего распознавания. Просто напиши мне \"послезавтра в 9 вечера запись к барберу\"" });
        }

        public async Task AskTimeZone(long chatId)
        {
            //await this._senderService.SendMessageWithButtonsAsync(chatId);
            throw new NotImplementedException();
        }
    }
}