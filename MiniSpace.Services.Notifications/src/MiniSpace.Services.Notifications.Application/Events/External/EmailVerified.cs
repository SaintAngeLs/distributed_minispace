using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application.Events.External
{
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
