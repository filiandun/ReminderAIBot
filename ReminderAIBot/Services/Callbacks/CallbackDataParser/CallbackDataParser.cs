using ReminderAIBot.Models.Callbacks;
using ReminderAIBot.Models.Callbacks.Enums;
using ReminderAIBot.Models.Callbacks.Domains;


namespace ReminderAIBot.Services.Callbacks.CallbackDataParser
{
    public class CallbackDataParser : ICallbackDataParser
    {
        private readonly ILogger<CallbackDataParser> _logger; // TODO maybe он тут и не нужен


        public CallbackDataParser(ILogger<CallbackDataParser> logger)
        {
            this._logger = logger;
        }

        // TODO костыли какие-то, надо переделать
        public CallbackData? Parse(string data)
        {
            CallbackData? result = null;

            result = this.ParseOpenScreen(data);
            if (result is not null) return result;

            result = this.ParseChangePage(data);
            if (result is not null) return result;

            result = this.ParseReminder(data);
            if (result is not null) return result;

            result = this.ParseTimeZone(data);
            if (result is not null) return result;

            this._logger.LogWarning($"parse: unknown callbackdata ({data})");
            return null;
        }


        private OpenScreenCallbackData? ParseOpenScreen(string data)
        {
            var parts = data.Split(':');

            if (parts.Length != 2 || parts[0] != "open") return null;

            if (!int.TryParse(parts[1], out var screen)) return null;

            return new OpenScreenCallbackData { Screen = (UiScreen)screen };
        }

        private ChangePageCallbackData? ParseChangePage(string data)
        {
            var parts = data.Split(':');

            if (parts.Length != 2|| parts[0] != "page") return null;

            if (!int.TryParse(parts[1], out var page)) return null;

            return new ChangePageCallbackData { Page = page };
        }


        private ReminderCallbackData? ParseReminder(string data)
        {
            var parts = data.Split(':');

            if (parts.Length != 3 || parts[0] != "rem") return null;

            if (!int.TryParse(parts[1], out var action)) return null;

            if (!int.TryParse(parts[2], out var reminderId)) return null;

            return new ReminderCallbackData { Action = (ReminderAction)action, ReminderId = reminderId };
        }

        private ReminderCallbackData? ParseTimeZone(string data)
        {
            var parts = data.Split(':');

            if (parts.Length != 3 || parts[0] != "tmz") return null;

            if (!int.TryParse(parts[1], out var action)) return null;

            if (!int.TryParse(parts[2], out var reminderId)) return null;

            return new ReminderCallbackData { Action = (ReminderAction)action, ReminderId = reminderId };
        }
    }
}
