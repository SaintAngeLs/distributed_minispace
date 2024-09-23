using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
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