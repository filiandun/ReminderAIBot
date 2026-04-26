
namespace ReminderAIBot.Models.CallbackCommands
{
    public sealed record CreateReminderCommand : CallbackCommand;
    public sealed record EditReminderCommand(int ReminderId) : CallbackCommand;
    public sealed record DeleteReminderCommand(int ReminderId) : CallbackCommand;
}
