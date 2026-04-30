using ReminderAIBot.Models.Database;

using ReminderAIBot.Services.Repositories.ReminderRepository;
using ReminderAIBot.Services.Repositories.UserRepository;


namespace ReminderAIBot.Services.ReminderManager
{
    public class ReminderManager : IReminderManager
    {
        private readonly ILogger<ReminderManager> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IReminderRepository _reminderRepository;


        public ReminderManager(ILogger<ReminderManager> logger, IUserRepository userRepository, IReminderRepository reminderRepository)
        {
            this._logger = logger;

            this._userRepository = userRepository;
            this._reminderRepository = reminderRepository;
        }


        public async Task<Reminder> GetReminder(long platformUserId, int reminderId)
        {
            User? user = await this._userRepository.GetByPlatformUserId(platformUserId) ?? throw new Exception($"get user reminder: user by platform id [{platformUserId}] not found ");
            
            this._logger.LogTrace($"user {platformUserId} get reminder [{reminderId}]");

            Reminder reminder = await this._reminderRepository.GetReminder(reminderId) ?? throw new Exception($"get user reminder: reminder [{reminderId}] not found");
            if (reminder.UserId != user.Id) throw new Exception($"get user reminder: user [{user.Id}] try get reminder [{platformUserId}] other user");

            return reminder;
        }


        public async Task<List<Reminder>> GetRemindersList(long platformUserId)
        {
            User? user = await this._userRepository.GetByPlatformUserId(platformUserId);
            if (user is null) return new List<Reminder>();

            this._logger.LogTrace($"user {platformUserId} get all reminders");

            return await this._reminderRepository.GetRemindersList(user.Id);
        }

        public async Task AddReminder(long userId, Reminder reminder)
        {
            User? user = await this._userRepository.GetByPlatformUserId(userId);
            if (user is null)
            {
                user = new User { Id = new Random().Next(), PlatformUserId = userId, TimeZoneId = TimeZoneInfo.Local.ToString() };
                await this._userRepository.Add(user);
            }

            reminder.Id = new Random().Next();
            reminder.UserId = user.Id;

            this._logger.LogTrace($"add: user [{userId}] add new reminder {reminder.Text}");

            await this._reminderRepository.Add(reminder);
        }

        public async Task RemoveReminder(long userId, int reminderId)
        {
            User? user = await this._userRepository.GetByPlatformUserId(userId) ?? throw new Exception($"remove reminder: user by platform id [{reminderId}] not found");
            
            if (user.PlatformUserId != userId) throw new Exception("try remove reminder by other user");

            this._logger.LogTrace($"remove reminder: user [{userId}] remove reminder [{reminderId}]");

            await this._reminderRepository.Remove(reminderId);
        }
    }
}
