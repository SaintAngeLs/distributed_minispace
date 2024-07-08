using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Notifications.Application.Dto;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Notifications.Application.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                _logger.LogInformation($"======================================================================================User {userId} connected with connection ID: {Context.ConnectionId}");
            }
            else
            {
                _logger.LogWarning("=========================================================================================User ID is missing in the query string.");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                _logger.LogInformation($"User {userId} disconnected with connection ID: {Context.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            _logger.LogInformation($"Sending message to user {user}: {message}");
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToGroup(string groupName)
        {
            _logger.LogInformation($"Adding connection {Context.ConnectionId} to group {groupName}");
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            _logger.LogInformation($"Removing connection {Context.ConnectionId} from group {groupName}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendNotification(string userId, NotificationDto notification)
        {
            var jsonMessage = JsonSerializer.Serialize(notification);
            _logger.LogInformation($"Sending notification to user {userId}: {jsonMessage}");
            await Clients.User(userId).SendAsync("ReceiveNotification", jsonMessage);
        }

        public static async Task SendNotification(IHubContext<NotificationHub> hubContext, string userId, NotificationDto notification, ILogger logger)
        {
            var jsonMessage = JsonSerializer.Serialize(notification);
            logger.LogInformation($"Sending static notification to user {userId}: {jsonMessage}");
            await hubContext.Clients.User(userId).SendAsync("ReceiveNotification", jsonMessage);
        }

         public static async Task BroadcastNotification(IHubContext<NotificationHub> hubContext, NotificationDto notification, ILogger logger)
        {
            var jsonMessage = JsonSerializer.Serialize(notification);
            logger.LogInformation($"Broadcasting notification to all users: {jsonMessage}");
            await hubContext.Clients.All.SendAsync("ReceiveNotification", jsonMessage);
        }
    }
}
