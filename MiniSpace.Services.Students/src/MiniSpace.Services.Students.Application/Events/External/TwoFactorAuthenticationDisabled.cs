using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class TwoFactorAuthenticationDisabled : IEvent
    {
        public Guid UserId { get; }

        public TwoFactorAuthenticationDisabled(Guid userId)
        {
            UserId = userId;
        }
    }
}
