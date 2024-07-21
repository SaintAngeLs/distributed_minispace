// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.SignalR;
// using MiniSpace.Services.Notifications.Application.Services;
// using MiniSpace.Services.Notifications.Application.Hubs;
// using MiniSpace.Services.Notifications.Infrastructure.Hubs;

// namespace MiniSpace.Services.Notifications.Infrastructure.Managers
// {
//     public class SignalRConnectionManager : ISignalRConnectionManager
//     {
//         private readonly IHubContext<NotificationHub> _hubContext;

//         public SignalRConnectionManager(IHubContext<NotificationHub> hubContext)
//         {
//             _hubContext = hubContext;
//         }

//         public async Task SendMessageAsync(string user, string message)
//         {
//             await _hubContext.Clients.User(user).SendAsync("ReceiveMessage", message);
//         }
// }

// }