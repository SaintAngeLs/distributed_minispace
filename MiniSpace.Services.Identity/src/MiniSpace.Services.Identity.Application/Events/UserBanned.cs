using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Events
{
    public class UserBanned(Guid userId) : IEvent
    {
        public Guid UserId { get; } = userId;
    }
}