// INotificationHub.cs
using MiniSpace.Services.Notifications.Application.Dto;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Hubs
{
    public interface INotificationHub
    {
        Task SendMessage(string user, string message);
        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);
        Task SendNotification(string userId, NotificationDto notification);
    }
}
