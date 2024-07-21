using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class TwoFactorAuthenticationDisabled : IEvent
    {
        public Guid UserId { get; }

        public TwoFactorAuthenticationDisabled(Guid userId)
        {
            UserId = userId;
        }
    }
}
