using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IStudentNotificationsRepository
    {
        Task<StudentNotifications> GetByStudentIdAsync(Guid studentId);
        Task AddAsync(StudentNotifications studentNotifications);
        Task UpdateAsync(StudentNotifications studentNotifications);
        Task AddOrUpdateAsync(StudentNotifications studentNotifications);
        Task DeleteAsync(Guid studentId);
        Task UpdateNotificationStatus(Guid studentId, Guid notificationId, string newStatus);
        Task<bool> NotificationExists(Guid studentId, Guid notificationId);
    }
}
