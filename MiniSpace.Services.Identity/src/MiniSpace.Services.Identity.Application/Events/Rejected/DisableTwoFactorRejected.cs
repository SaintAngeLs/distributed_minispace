using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events.Rejected
{
    [Contract]
    public class DisableTwoFactorRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DisableTwoFactorRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
