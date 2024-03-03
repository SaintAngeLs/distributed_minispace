using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pacco.Services.Operations.Api.Hubs;
using Pacco.Services.Operations.Api.Infrastructure;

namespace Pacco.Services.Operations.Api.Services
{
    public class HubWrapper : IHubWrapper
    {
        private readonly IHubContext<PaccoHub> _hubContext;

        public HubWrapper(IHubContext<PaccoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(string userId, string message, object data)
            => await _hubContext.Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

        public async Task PublishToAllAsync(string message, object data)
            => await _hubContext.Clients.All.SendAsync(message, data);
    }
}