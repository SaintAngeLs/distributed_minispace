using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    [Contract]
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
