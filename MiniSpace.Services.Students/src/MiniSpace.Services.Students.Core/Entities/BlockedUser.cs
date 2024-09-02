using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class BlockedUser
    {
        public Guid BlockerId { get; private set; }
        public Guid BlockedUserId { get; private set; }
        public DateTime BlockedAt { get; private set; }

        public BlockedUser(Guid blockerId, Guid blockedUserId, DateTime blockedAt)
        {
            BlockerId = blockerId;
            BlockedUserId = blockedUserId;
            BlockedAt = blockedAt;
        }
    }
}
