namespace ReminderAIBot.Services.OnboardingService
{
    public interface IOnboardingService
    {
        public Task Greeting(long chatId);
    }
}
