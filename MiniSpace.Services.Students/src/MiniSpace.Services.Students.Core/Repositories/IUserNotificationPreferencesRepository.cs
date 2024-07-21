using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserNotificationPreferencesRepository
    {
        Task<NotificationPreferences> GetNotificationPreferencesAsync(Guid studentId);
        Task UpdateNotificationPreferencesAsync(Guid studentId, NotificationPreferences notificationPreferences);
    }
}
