using ReminderAIBot.Models.Callbacks;
using System.Runtime.CompilerServices;


namespace ReminderAIBot.Services.Callbacks.CallbackDataCodec
{
    public static class CallbackDataCodec
    {
        private const string NavigationPrefix = "nav";
        private const string ReminderPrefix = "rem";
        private const string SettingsPrefix = "set";


        public static string Encode(CallbackData data)
        {
            return data switch
            {
                NavigationCallbackData navigationCallbackData => CallbackDataCodec.EncodeNavigation(navigationCallbackData),
                ReminderCallbackData reminderCallbackData => CallbackDataCodec.EncodeReminder(reminderCallbackData),
                SettingsCallbackData settingsCallbackData => CallbackDataCodec.EncodeSettings(settingsCallbackData),

                _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };
        }

        public static string Encode(Screen screen, NavigationAction action, int? targetPage = null) => CallbackDataCodec.Encode(new NavigationCallbackData() { Screen = screen, Action = action, TargetPage = targetPage});
        public static string Encode(ReminderAction action, int? reminderId = null) => CallbackDataCodec.Encode(new ReminderCallbackData() { Action = action, ReminderId = reminderId });
        public static string Encode(SettingsAction action, string? timeZoneId = null) => CallbackDataCodec.Encode(new SettingsCallbackData() { Action = action, TimeZoneId = timeZoneId });


        public static CallbackData? Decode(string data)
        {
            string[] parts = data.Split(':');

            if (parts.Length == 0) return null;

            return parts[0] switch
            {
                CallbackDataCodec.NavigationPrefix => CallbackDataCodec.DecodeNavigation(parts),
                CallbackDataCodec.ReminderPrefix => CallbackDataCodec.DecodeReminder(parts),
                CallbackDataCodec.SettingsPrefix => CallbackDataCodec.DecodeSettings(parts),

                _ => null
            };
        }



        private static string EncodeNavigation(NavigationCallbackData data)
        {
            if (data.TargetPage is null) return $"{CallbackDataCodec.ToToken(data)}:{CallbackDataCodec.ToToken(data.Screen)}:{CallbackDataCodec.ToToken(data.Action)}";

            return $"{CallbackDataCodec.ToToken(data)}:{CallbackDataCodec.ToToken(data.Screen)}:{CallbackDataCodec.ToToken(data.Action)}:{(int)data.TargetPage}";
        }

        private static string EncodeReminder(ReminderCallbackData callbackData)
        {
            if (callbackData.ReminderId is null) return $"{CallbackDataCodec.ToToken(callbackData)}:{CallbackDataCodec.ToToken(callbackData.Action)}";

            return $"{CallbackDataCodec.ToToken(callbackData)}:{CallbackDataCodec.ToToken(callbackData.Action)}:{(int)callbackData.ReminderId}";
        }

        private static string EncodeSettings(SettingsCallbackData callbackData)
        {
            if (callbackData.TimeZoneId is null) return $"{CallbackDataCodec.ToToken(callbackData)}:{CallbackDataCodec.ToToken(callbackData.Action)}";

            return $"{CallbackDataCodec.ToToken(callbackData)}:{CallbackDataCodec.ToToken(callbackData.Action)}:{callbackData.TimeZoneId}";
        }



        private static NavigationCallbackData? DecodeNavigation(string[] parts)
        {
            if (parts.Length < 3) return null;

            if (!CallbackDataCodec.TryParseNavigationScreen(parts[1], out Screen screen)) return null;

            if (!CallbackDataCodec.TryParseNavigationAction(parts[2], out NavigationAction action)) return null;

            if (parts.Length == 4 && int.TryParse(parts[3], out int targetPage)) return new NavigationCallbackData() { Screen = screen, Action = action, TargetPage = targetPage };

            return new NavigationCallbackData() { Screen = screen, Action = action };
        }

        private static ReminderCallbackData? DecodeReminder(string[] parts)
        {
            if (parts.Length < 2) return null;

            if (!CallbackDataCodec.TryParseReminderAction(parts[1], out ReminderAction action)) return null;

            if (parts.Length == 3 && int.TryParse(parts[2], out int reminderId)) return new ReminderCallbackData() { Action = action, ReminderId = reminderId };

            return new ReminderCallbackData() { Action = action };
        }

        private static SettingsCallbackData? DecodeSettings(string[] parts)
        {
            if (parts.Length < 2) return null;

            if (!CallbackDataCodec.TryParseSettingsAction(parts[1], out SettingsAction action)) return null;

            if (parts.Length == 3) return new SettingsCallbackData() { Action = action, TimeZoneId = parts[2] };

            return new SettingsCallbackData() { Action = action };
        }



        private static string ToToken(CallbackData data)
        {
            return data switch
            {
                NavigationCallbackData => CallbackDataCodec.NavigationPrefix,
                ReminderCallbackData => CallbackDataCodec.ReminderPrefix,
                SettingsCallbackData => CallbackDataCodec.SettingsPrefix,

                _ => throw new ArgumentOutOfRangeException(nameof(data), data, null)
            };
        }

        private static string ToToken(NavigationAction action)
        {
            return action switch
            {
                NavigationAction.Open => "open",
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
        }

        private static string ToToken(Screen screen)
        {
            return screen switch
            {
                Screen.Home => "home",

                Screen.CreateReminder => "createreminder",
                Screen.EditReminder => "editreminder",

                Screen.RemindersList => "reminderslist",
                Screen.TimeZonesList => "timezoneslist",

                Screen.Settings => "settings",

                _ => throw new ArgumentOutOfRangeException(nameof(screen), screen, null)
            };
        }

        private static string ToToken(ReminderAction action)
        {
            return action switch
            {
                ReminderAction.Create => "create",
                ReminderAction.Edit => "edit",
                ReminderAction.Delete => "delete",

                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
        }

        private static string ToToken(SettingsAction action)
        {
            return action switch
            {
                SettingsAction.SetTimeZone => "settimezone",

                _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
        }



        private static bool TryParseNavigationScreen(string part, out Screen screen)
        {
            switch (part)
            {
                case "home": screen = Screen.Home; break;

                case "createreminder": screen = Screen.CreateReminder; break;
                case "editreminder": screen = Screen.EditReminder; break;

                case "reminderslist": screen = Screen.RemindersList; break;
                case "timezoneslist": screen = Screen.TimeZonesList; break;

                case "settings": screen = Screen.Settings; break;

                default: screen = Screen.Home; return false;
            };

            return true;
        }

        private static bool TryParseNavigationAction(string part, out NavigationAction action)
        {
            switch (part)
            {
                case "open": action = NavigationAction.Open; break;

                default: action = NavigationAction.Open; return false;
            };

            return true;
        }

        private static bool TryParseReminderAction(string part, out ReminderAction action)
        {
            switch (part)
            {
                case "create": action = ReminderAction.Create; break;
                case "edit": action = ReminderAction.Edit; break;
                case "delete": action = ReminderAction.Delete; break;

                default: action = ReminderAction.Create; return false;
            };

            return true;
        }

        private static bool TryParseSettingsAction(string part, out SettingsAction action)
        {
            switch (part)
            {
                case "settimezone": action = SettingsAction.SetTimeZone; break;

                default: action = SettingsAction.SetTimeZone; return false;
            };

            return true;
        }
    }
}