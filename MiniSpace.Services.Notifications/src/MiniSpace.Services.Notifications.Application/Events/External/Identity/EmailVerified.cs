using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Notifications.Application.Events.External.Identity
{
    [Message("identity")]
    public class EmailVerified : IEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public DateTime VerifiedAt { get; }

        public EmailVerified(Guid userId, string email, DateTime verifiedAt)
        {
            UserId = userId;
            Email = email;
            VerifiedAt = verifiedAt;
        }
    }
}
