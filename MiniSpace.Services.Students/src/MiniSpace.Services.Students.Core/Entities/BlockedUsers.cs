using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Students.Core.Events;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class BlockedUsers : AggregateRoot
    {
        private readonly ISet<BlockedUser> _blockedUsers = new HashSet<BlockedUser>();
        public Guid UserId { get; private set; }
        public IEnumerable<BlockedUser> BlockedUsersList => _blockedUsers;

        public BlockedUsers(Guid userId)
        {
            UserId = userId;
        }

        public void BlockUser(Guid blockedUserId)
        {
            if (blockedUserId == Guid.Empty || _blockedUsers.Any(b => b.BlockedUserId == blockedUserId))
            {
                throw new InvalidOperationException($"User with ID {blockedUserId} cannot be blocked or is already blocked.");
            }

            var blockedUser = new BlockedUser(UserId, blockedUserId, DateTime.UtcNow);
            _blockedUsers.Add(blockedUser);
            AddEvent(new UserBlockedEvent(this, blockedUserId));
        }

        public void UnblockUser(Guid blockedUserId)
        {
            var blockedUser = _blockedUsers.SingleOrDefault(b => b.BlockedUserId == blockedUserId);
            if (blockedUser == null)
            {
                throw new InvalidOperationException($"User with ID {blockedUserId} is not blocked.");
            }

            _blockedUsers.Remove(blockedUser);
            AddEvent(new UserUnblockedEvent(this, blockedUserId));
        }
    }
}
