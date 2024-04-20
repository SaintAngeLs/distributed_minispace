using System;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class UserUnbanned(Guid userId) : IEvent
    {
        public Guid UserId { get; } = userId;
    }
}