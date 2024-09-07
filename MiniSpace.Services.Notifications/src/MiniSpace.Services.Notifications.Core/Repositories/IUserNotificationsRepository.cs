using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IUserNotificationsRepository
    {
        Task<UserNotifications> GetByUserIdAsync(Guid userId);
        Task AddAsync(UserNotifications userNotifications);
        Task UpdateAsync(UserNotifications userNotifications);
        Task AddOrUpdateAsync(UserNotifications userNotifications);
        Task DeleteAsync(Guid userId);
        Task UpdateNotificationStatus(Guid userId, Guid notificationId, string newStatus);
        Task<bool> NotificationExists(Guid userId, Guid notificationId);
        Task DeleteNotification(Guid userId, Guid notificationId);
    }
}
