using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
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
