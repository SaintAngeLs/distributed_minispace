using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserBlockedEvent : IDomainEvent
    {
        public BlockedUsers BlockedUsers { get; }
        public Guid BlockedUserId { get; }

        public UserBlockedEvent(BlockedUsers blockedUsers, Guid blockedUserId)
        {
            BlockedUsers = blockedUsers;
            BlockedUserId = blockedUserId;
        }
    }
}
