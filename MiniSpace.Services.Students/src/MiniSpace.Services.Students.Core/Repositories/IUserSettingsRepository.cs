using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserSettingsRepository
    {
        Task<UserSettings> GetUserSettingsAsync(Guid studentId);
        Task AddUserSettingsAsync(UserSettings userSettings);
        Task UpdateUserSettingsAsync(UserSettings userSettings);
    }
}
