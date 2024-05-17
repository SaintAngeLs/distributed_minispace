using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Entities;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IStudentNotificationsRepository
    {
        Task<StudentNotifications> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<Notification>> GetNotificationsAsync(Guid studentId);
        Task AddNotificationAsync(Guid studentId, Notification notification);
        Task UpdateNotificationAsync(Guid studentId, Notification notification);
        Task RemoveNotificationAsync(Guid studentId, Guid notificationId);
    }
}
