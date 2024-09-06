using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class BlockUser : ICommand
    {
        public Guid BlockerId { get; }
        public Guid BlockedUserId { get; }

        public BlockUser(Guid blockerId, Guid blockedUserId)
        {
            BlockerId = blockerId;
            BlockedUserId = blockedUserId;
        }
    }
}
