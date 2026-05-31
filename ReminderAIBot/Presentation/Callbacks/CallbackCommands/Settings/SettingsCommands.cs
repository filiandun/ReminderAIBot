
namespace ReminderAIBot.Presentation.Callbacks.CallbackCommands.Settings
{
    public sealed record OpenTimeZoneListCommand(int Page) : CallbackCommand;

    public record SetTimeZoneCommand(string TimeZoneId) : CallbackCommand;
}
