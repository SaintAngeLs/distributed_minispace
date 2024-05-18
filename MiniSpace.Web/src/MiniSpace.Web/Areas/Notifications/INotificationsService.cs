using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Notifications;

namespace MiniSpace.Web.Areas.Notifications
{
    public interface INotificationsService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(Guid userId);
        // Task<NotificationDto> CreateNotificationAsync(NotificationDto notification);
        Task UpdateNotificationStatusAsync(Guid notificationId, string status);
        Task DeleteNotificationAsync(Guid notificationId);
    }
}
