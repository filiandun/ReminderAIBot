using Microsoft.AspNetCore.Mvc;

using Telegram.Bot;
using Telegram.Bot.Types;


namespace ReminderAIBot.Controllers
{
    [ApiController]
    [Route("api/telegram/update")]
    public class TelegramBotWebhookController : ControllerBase
    {
        private readonly ILogger<TelegramBotWebhookController> _logger;

        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramBotWebhookController(ITelegramBotClient botClient, ILogger<TelegramBotWebhookController> logger)
        {
            this._logger = logger;
            this._telegramBotClient = botClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            //this._logger.LogInformation($"New message: {update.Message.Text}");
            if (update.Message?.Text == "/start")
            {
                await this._telegramBotClient.SendMessage(update.Message.Chat.Id, "Привет!");
            }

            return Ok();
        }
    }
}
