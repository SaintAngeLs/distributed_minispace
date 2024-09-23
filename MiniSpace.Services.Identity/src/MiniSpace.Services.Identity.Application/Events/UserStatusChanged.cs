using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    [Contract]
    public class UserStatusChanged : IEvent
    {
        public Guid UserId { get; }
        public bool IsOnline { get; }
        public string DeviceType { get; }
        public string IpAddress { get; }

        public UserStatusChanged(Guid userId, bool isOnline, string deviceType, string ipAddress)
        {
            UserId = userId;
            IsOnline = isOnline;
            DeviceType = deviceType;
            IpAddress = ipAddress;
        }
    }
}
