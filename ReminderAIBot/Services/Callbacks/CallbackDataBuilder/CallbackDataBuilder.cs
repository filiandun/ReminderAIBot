using ReminderAIBot.Models.Callbacks;


namespace ReminderAIBot.Services.Callbacks.CallbackDataBuilder
{
    public class CallbackDataBuilder : ICallbackDataBuilder
    {
        public string Build(Screen screen, ScreenAction action)
        {
            //string screenStr = screen switch
            //{
            //    Screen.Home => "home",

            //    Screen.RemindersList => "remlist",
            //    Screen.TimeZonesList => "tmzlist",

            //    _ => throw new Exception("build: unknown callback")

            //};

            //string actionStr = action switch
            //{
            //    ScreenAction.Open => "open",

            //    ScreenAction.NextPage => "next",
            //    ScreenAction.PrevPage => "prev",

            //    _ => throw new Exception("build: unknown callback")
            //};

            //return $"{screenStr}:{actionStr}";

            return $"{(int)screen}:{(int)action}";
        }
    }
}
