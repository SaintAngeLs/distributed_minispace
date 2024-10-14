using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Notifications;

namespace MiniSpace.Web.Areas.Notifications
{
    public interface INotificationsService
    {
        Task<PaginatedResponseDto<NotificationDto>> GetNotificationsByUserAsync(Guid userId, int page = 1, int pageSize = 10, string sortOrder = "desc");
        Task<PaginatedResponseDto<NotificationDto>> GetNotificationsByUserAsync(Guid userId, int page = 1, int pageSize = 10, string sortOrder = "desc", string status = "Unread");
    
        Task UpdateNotificationStatusAsync(Guid userId, Guid notificationId, string status);

        Task UpdateNotificationStatusAsync(Guid notificationId, bool isActive); 
        Task DeleteNotificationAsync(Guid userId, Guid notificationId);
        Task<NotificationDto> GetNotificationByIdAsync(Guid userId, Guid notificationId);
        Task CreateNotificationAsync(NotificationToUsersDto notification);
        Task<bool> IsUserConnectedAsync(Guid userId);
    }
}
