using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IUserNotificationPreferencesRepository
    {
        Task<NotificationPreferences> GetNotificationPreferencesAsync(Guid userId);
        Task UpdateNotificationPreferencesAsync(Guid userId, NotificationPreferences notificationPreferences);
    }
}
