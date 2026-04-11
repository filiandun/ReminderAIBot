using ReminderAIBot.Models.CallbackData;
using ReminderAIBot.Models.CallbackData.Enums;

namespace ReminderAIBot.Services.CallbackDataParser
{
    public class CallbackDataParser : ICallbackDataParser
    {
        private readonly ILogger<CallbackDataParser> _logger;


        public CallbackDataParser(ILogger<CallbackDataParser> logger)
        {
            this._logger = logger;
        }


        public CallbackReminder? ParseReminder(string callbackData)
        {
            var parts = callbackData.Split(':');

            if (parts.Length != 3 || parts[0] != "rem") return null;

            if (!int.TryParse(parts[1], out var action)) return null;

            if (!int.TryParse(parts[2], out var id)) return null;

            return new CallbackReminder { Action = (CallbackReminderAction)action, Id = id };
        }
    }
}
