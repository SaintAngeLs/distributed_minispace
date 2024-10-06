using System;
using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
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
