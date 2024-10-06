using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Students.Application.Events
{
    public class UserBlocked : IEvent
    {
        public Guid BlockerId { get; }
        public Guid BlockedUserId { get; }

        public UserBlocked(Guid blockerId, Guid blockedUserId)
        {
            BlockerId = blockerId;
            BlockedUserId = blockedUserId;
        }
    }
}
