using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IStudentNotificationsRepository
    {
        Task<StudentNotifications> GetByStudentIdAsync(Guid userId);
        Task AddAsync(StudentNotifications studentNotifications);
        Task UpdateAsync(StudentNotifications studentNotifications);
        Task AddOrUpdateAsync(StudentNotifications studentNotifications);
        Task DeleteAsync(Guid userId);
        Task UpdateNotificationStatus(Guid userId, Guid notificationId, string newStatus);
        Task<bool> NotificationExists(Guid userId, Guid notificationId);
        Task DeleteNotification(Guid userId, Guid notificationId);
    }
}
