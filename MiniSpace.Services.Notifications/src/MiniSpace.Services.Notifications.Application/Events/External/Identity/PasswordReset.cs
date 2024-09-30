using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity
{
    [Message("identity")]
    public class PasswordReset : IEvent
    {
        public Guid UserId { get; }

        public PasswordReset(Guid userId)
        {
            UserId = userId;
        }
    }
}
