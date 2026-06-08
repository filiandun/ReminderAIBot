using System.Text.Json;

using IAIChatServiceLib;
using IAIChatServiceLib.Models;

using ReminderAIBot.Domain;
using ReminderAIBot.Application.Ports;


namespace ReminderAIBot.Infrastructure.ReminderParser
{
    public class ReminderParser : IReminderParser
    {
        private readonly ILogger<ReminderParser> _logger;

        private readonly IAIChatService _AIChatService;

        private string _template = "Ты — сервис для парсинга напоминаний." +
                            "Твоя задача: преобразовать текст пользователя в структурированный JSON." +
                            "Правила:" +
                            "1. Отвечай ТОЛЬКО валидным JSON, без пояснений, без текста до и после, без комментариев //." +
                            "2. Если данных недостаточно — заполняй поля null." +
                            "3. Пользователь пишет время в своем локальном часовом поясе. Конвертируй его в UTC." +
                            "4. Поле RemindAtUtc должно быть в ISO 8601 формате UTC: YYYY-MM-DDTHH:mm:ssZ." +
                            "5. Если пользователь не указал дату, но указал время — используй ближайшую будущую дату в его часовом поясе." +
                            "6. Если указано \"сегодня\", \"завтра\", \"через X минут/часов\" — корректно интерпретируй относительно локального времени пользователя." +
                            "7. Текущая дата и время пользователя: {{CURRENT_DATETIME}}" +
                            "8. Часовой пояс пользователя: {{CURRENT_TIMEZONE}}" +
                            "Формат ответа:\r\n{\r\n" +
                            "  \"Text\": \"string\",\r\n" +
                            "  \"RemindAtUtc\": \"string|null\",\r\n" +
                            "  \"IsValid\": true|false\r\n" +
                            "}\r\n\r\n" +
                            "Примеры:" +
                            "\r\n\r\n" +
                            "Вход:\r\n\"Напомни купить молоко завтра в 16:00\"\r\n\r\n" +
                            "Выход:\r\n{\r\n" +
                            "  \"Text\": \"купить молоко\",\r\n" +
                            "  \"RemindAtUtc\": \"2026-03-28T13:00:00Z\",\r\n" +
                            "  \"IsValid\": true\r\n" +
                            "}\r\n\r\n" +
                            "Вход:\r\n\"Позвонить маме\"\r\n\r\n" +
                            "Выход:\r\n{\r\n" +
                            "  \"Text\": \"позвонить маме\",\r\n" +
                            "  \"RemindAtUtc\": null,\r\n" +
                            "  \"IsValid\": false\r\n" +
                            "}\r\n\r\n" +
                            "Теперь обработай сообщение пользователя:\r\n\"{{USER_INPUT}}\"";


        public ReminderParser(ILogger<ReminderParser> logger, IAIChatService AIChatService)
        {
            this._logger = logger;

            this._AIChatService = AIChatService;
        }

        // TODO добавить базовый парсинг, чтобы не дёргать AI, кодгда сообщение приходят уровня "как дела?".
        public async Task<Reminder> ParseAsync(string rawText)
        {
            int balance = await this._AIChatService.GetBalanceTokenAsync();
            this._logger.LogInformation($"balance - {balance}");

            AIRequest AIRequest = this.MapToAIRequest("user", rawText);

            AIResponse AIResponse = await this._AIChatService.GetResponseAsync(AIRequest);

            return this.MapToReminder(AIResponse, rawText);
        }

        // TODO Replace("{{CURRENT_TIMEZONE}}" сделать, чтобы брался пользователя
        private AIRequest MapToAIRequest(string role, string text)
        {
            AIRequest AIRequest = new AIRequest()
            {
                Messages = new List<AIMessage>() 
                { 
                    new AIMessage() { Role = "system", Content = this._template.Replace("{{USER_INPUT}}", text).Replace("{{CURRENT_DATETIME}}", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")).Replace("{{CURRENT_TIMEZONE}}", "Europe/Moscow") },
                }
            };

            return AIRequest;
        }

        // TODO очень важно проверять валидность JSON и требовать заново сгенерировать от ИИ:
        // очень часто он любит добавить комментарий или какие-нибудь ещё символы добавить
        // пример метода для очистки в самом низу (CleanJson)
        private Reminder MapToReminder(AIResponse AIResponse, string rawText)
        {
            string json = this.CleanJson(AIResponse.Message.Content);

            this._logger.LogInformation("parse async: clear json\n{json}", json);

            // TODO добавить try catch
            ReminderDraft? reminderDraft = JsonSerializer.Deserialize<ReminderDraft>(json);

            if (reminderDraft is null)
            {
                return new Reminder();
            }

            Reminder reminder = new Reminder()
            {
                RemindAtUtc = reminderDraft.RemindAtUtc,
                CreatedAtUtc = DateTime.UtcNow,
                RawText = rawText,
                Text = reminderDraft.Text,
            };

            return reminder;
        }


        private string CleanJson(string input)
        {
            input = input.Replace("'```json", "").Replace("```", "");

            int start = input.IndexOf('{');
            if (start >= 0)
                input = input[start..];

            int end = input.LastIndexOf('}');
            if (end >= 0)
                input = input[..(end + 1)];

            return input.Trim();
        }
    }
}
