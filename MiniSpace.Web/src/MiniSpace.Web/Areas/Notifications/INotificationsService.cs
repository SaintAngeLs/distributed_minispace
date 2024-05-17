using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Notifications;

namespace MiniSpace.Web.Areas.Notifications
{
    public interface INotificationsService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(string userId);
        // Task<NotificationDto> CreateNotificationAsync(NotificationDto notification);
        Task UpdateNotificationStatusAsync(string notificationId, string status);
        Task DeleteNotificationAsync(string notificationId);
    }
}
