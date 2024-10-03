using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
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