using ReminderAIBot.Models.Database;

namespace ReminderAIBot.Services.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<User?> GetByPlatformUserId(long platformUserId);

        public Task Add(User user);
        public Task Remove(User user);
        public Task Update(User user);
    }
}
