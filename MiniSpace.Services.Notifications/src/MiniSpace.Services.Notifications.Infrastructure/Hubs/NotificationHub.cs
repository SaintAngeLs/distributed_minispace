using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MiniSpace.Services.Notifications.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        // This method can be called by the client to send a message
        public async Task SendMessage(string user, string message)
        {
            // Call the ReceiveMessage method on the client
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        // You can also add methods to manage group memberships
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}