using ReminderAIBot.Models.Callbacks;


namespace ReminderAIBot.Services.Callbacks.CallbackDataParser
{
    public class CallbackDataParser : ICallbackDataParser
    {
        private readonly ILogger<CallbackDataParser> _logger; // TODO maybe он тут и не нужен


        public CallbackDataParser(ILogger<CallbackDataParser> logger)
        {
            this._logger = logger;
        }


        public CallbackData? Parse(string data)
        {
            var parts = data.Split(':');

            if (!int.TryParse(parts[0], out var screen)) return null;

            if (!int.TryParse(parts[1], out var action)) return null;

            CallbackData? callbackData = new CallbackData() { Screen = (Screen)screen, Action = (ScreenAction)action};

            return callbackData;
        }
    }
}
