using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    
    public class TokenRefreshed : IEvent
    {
        public Guid UserId { get; }
        public string DeviceType { get; }
        public string IpAddress { get; }

        public TokenRefreshed(Guid userId, string deviceType, string ipAddress)
        {
            UserId = userId;
            DeviceType = deviceType;
            IpAddress = ipAddress;
        }
    }
}
