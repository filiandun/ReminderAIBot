
namespace ReminderAIBot.Presentation.Callbacks.CallbackCommands.Reminder
{
    public sealed record OpenRemindersListCommand(int Page) : CallbackCommand;
    public sealed record OpenReminderDetailsCommand(int ReminderId) : CallbackCommand;

    public sealed record CreateReminderCommand : CallbackCommand;
    public sealed record EditReminderCommand(int ReminderId) : CallbackCommand;
    public sealed record DeleteReminderCommand(int ReminderId) : CallbackCommand;
}
