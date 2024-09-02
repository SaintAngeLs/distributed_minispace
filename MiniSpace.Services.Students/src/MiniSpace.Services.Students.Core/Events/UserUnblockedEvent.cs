using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserUnblockedEvent : IDomainEvent
    {
        public BlockedUsers BlockedUsers { get; }
        public Guid UnblockedUserId { get; }

        public UserUnblockedEvent(BlockedUsers blockedUsers, Guid unblockedUserId)
        {
            BlockedUsers = blockedUsers;
            UnblockedUserId = unblockedUserId;
        }
    }
}
