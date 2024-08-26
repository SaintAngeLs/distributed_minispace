using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Communication.Application.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                Users.TryAdd(Context.ConnectionId, username);
                await Clients.All.SendAsync("UserConnected", username);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Users.TryRemove(Context.ConnectionId, out var username);
            if (!string.IsNullOrEmpty(username))
            {
                await Clients.All.SendAsync("UserDisconnected", username);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToUser(string targetUser, string message)
        {
            var connectionId = Users.FirstOrDefault(u => u.Value == targetUser).Key;
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
            }
        }

        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", Context.User.Identity.Name, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.User.Identity.Name} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ShowWho", $"{Context.User.Identity.Name} has left the group {groupName}.");
        }
    }
}
