using System;
using Paralax.CQRS.Events;
using MiniSpace.Services.Identity.Core.Entities;

namespace MiniSpace.Services.Identity.Application.Events
{
    
    public class SignedIn : IEvent
    {
        public Guid UserId { get; }
        public string Role { get; }
        public string DeviceType { get; }  
        public string IpAddress { get; } 

        public SignedIn(Guid userId, string role, string deviceType, string ipAddress)
        {
            UserId = userId;
            Role = role;
            DeviceType = deviceType;  
            IpAddress = ipAddress;
        }
    }
}
