// using Microsoft.AspNetCore.SignalR;
// using MiniSpace.Services.Notifications.Application.Dto;
// using MiniSpace.Services.Notifications.Application.Hubs;
// using Microsoft.Extensions.Logging;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace MiniSpace.Services.Notifications.Infrastructure.Hubs
// {
//     public class NotificationHub : Hub, INotificationHub
//     {
//         private readonly ILogger<NotificationHub> _logger;

//         public NotificationHub(ILogger<NotificationHub> logger)
//         {
//             _logger = logger;
//         }

//         public async Task SendMessage(string user, string message)
//         {
//             _logger.LogInformation($"Sending message to user {user}: {message}");
//             await Clients.User(user).SendAsync("ReceiveMessage", message);
//         }

//         public async Task AddToGroup(string groupName)
//         {
//             _logger.LogInformation($"Adding connection {Context.ConnectionId} to group {groupName}");
//             await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
//         }

//         public async Task RemoveFromGroup(string groupName)
//         {
//             _logger.LogInformation($"Removing connection {Context.ConnectionId} from group {groupName}");
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
//         }

//         public async Task SendNotification(string userId, NotificationDto notification)
//         {
//             if (Clients == null)
//             {
//                 _logger.LogError("SignalR Clients is null. Cannot send notification.");
//                 return;
//             }

//             var jsonMessage = JsonSerializer.Serialize(notification);
//             _logger.LogInformation($"Sending notification to user {userId}: {jsonMessage}");
//             await Clients.User(userId).SendAsync("ReceiveNotification", jsonMessage);
//         }

//         public override Task OnConnectedAsync()
//         {
//             _logger.LogInformation($"Connection established: {Context.ConnectionId}");
//             return base.OnConnectedAsync();
//         }

//         public override Task OnDisconnectedAsync(Exception exception)
//         {
//             _logger.LogInformation($"Connection disconnected: {Context.ConnectionId}");
//             return base.OnDisconnectedAsync(exception);
//         }
//     }
// }
