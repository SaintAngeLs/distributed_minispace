using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Students.Application.Hubs;

public class PresenceHub : Hub
{
    private readonly ILogger<PresenceHub> _logger;

    public PresenceHub(ILogger<PresenceHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "OnlineUsers");
            _logger.LogInformation($"User {userId} connected with Connection ID: {Context.ConnectionId}");

            await Clients.All.SendAsync("UserOnline", userId);
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
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "OnlineUsers");
            _logger.LogInformation($"User {userId} disconnected with Connection ID: {Context.ConnectionId}");

            await Clients.All.SendAsync("UserOffline", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}