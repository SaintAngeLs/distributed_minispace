using Microsoft.AspNetCore.SignalR;
using MiniSpace.Services.Communication.Application.Dto;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Communication.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                _logger.LogInformation($"User {userId} connected and added to group with Connection ID: {Context.ConnectionId}");
            }
            else
            {
                _logger.LogWarning("User ID is missing in the query string.");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                _logger.LogInformation($"User {userId} disconnected and removed from group with Connection ID: {Context.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userId, MessageDto message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            _logger.LogInformation($"Sending message to user {userId}: {jsonMessage}");
            await Clients.User(userId).SendAsync("ReceiveMessage", jsonMessage);
        }

        public async Task SendMessageToGroup(string groupName, MessageDto message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            _logger.LogInformation($"Sending message to group {groupName}: {jsonMessage}");
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", jsonMessage);
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

        public static async Task SendMessageToUser(IHubContext<ChatHub> hubContext, string userId, MessageDto message, ILogger logger)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            logger.LogInformation($"Sending message to user {userId}: {jsonMessage}");
            await hubContext.Clients.All.SendAsync("ReceiveMessage", jsonMessage);
        }

        public static async Task BroadcastMessage(IHubContext<ChatHub> hubContext, MessageDto message, ILogger logger)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            logger.LogInformation($"Broadcasting message to all users: {jsonMessage}");
            await hubContext.Clients.All.SendAsync("ReceiveMessage", jsonMessage);
        }


        public static async Task SendMessageStatusUpdate(IHubContext<ChatHub> hubContext, string chatId, Guid messageId, string status, ILogger logger)
        {
            var statusUpdate = new
            {
                ChatId = chatId,
                MessageId = messageId,
                Status = status
            };

            logger.LogInformation($"Sending message status update to chat {chatId} for message {messageId} with status {status}");

            var jsonStatusUpdate = JsonSerializer.Serialize(statusUpdate);

            await hubContext.Clients.All.SendAsync("ReceiveMessageStatusUpdate", jsonStatusUpdate);
        }

        public async Task SendTypingNotification(string chatId, string userId, bool isTyping)
        {
            _logger.LogInformation($"User {userId} is typing in chat {chatId}: {isTyping}");
            await Clients.All.SendAsync("ReceiveTypingNotification", userId, isTyping);
        }

    }
}
