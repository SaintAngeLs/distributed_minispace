using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class TwoFactorAuthenticationEnabled : IEvent
    {
        public Guid UserId { get; }
        public string Secret { get; }

        public TwoFactorAuthenticationEnabled(Guid userId, string secret)
        {
            UserId = userId;
            Secret = secret;
        }
    }
}
