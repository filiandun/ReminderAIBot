
namespace ReminderAIBot.Models.CallbackCommands
{
    public sealed record OpenHomeCommand() : CallbackCommand;

    public sealed record OpenRemindersListCommand(int Page) : CallbackCommand;
    public sealed record OpenReminderDetailsCommand(int ReminderId) : CallbackCommand;

    public sealed record OpenTimeZoneListCommand(int Page) : CallbackCommand;
}
