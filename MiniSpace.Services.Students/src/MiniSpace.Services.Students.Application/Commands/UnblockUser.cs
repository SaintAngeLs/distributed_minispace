using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UnblockUser : ICommand
    {
        public Guid BlockerId { get; }
        public Guid BlockedUserId { get; }

        public UnblockUser(Guid blockerId, Guid blockedUserId)
        {
            BlockerId = blockerId;
            BlockedUserId = blockedUserId;
        }
    }
}
