using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    
    public class SignedOut : IEvent
    {
        public Guid UserId { get; }
        public string DeviceType { get; }

        public SignedOut(Guid userId, string deviceType)
        {
            UserId = userId;
            DeviceType = deviceType;
        }
    }
}
