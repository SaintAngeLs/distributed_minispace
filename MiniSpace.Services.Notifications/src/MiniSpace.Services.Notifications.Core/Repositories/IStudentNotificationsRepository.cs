using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IStudentNotificationsRepository
    {
        Task<UserNotifications> GetByStudentIdAsync(Guid userId);
        Task AddAsync(UserNotifications studentNotifications);
        Task UpdateAsync(UserNotifications studentNotifications);
        Task AddOrUpdateAsync(UserNotifications studentNotifications);
        Task DeleteAsync(Guid userId);
        Task UpdateNotificationStatus(Guid userId, Guid notificationId, string newStatus);
        Task<bool> NotificationExists(Guid userId, Guid notificationId);
        Task DeleteNotification(Guid userId, Guid notificationId);
    }
}
