using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class RevokeOrganizerRights : ICommand
    {
        public Guid UserId { get; }

        public RevokeOrganizerRights(Guid userId)
        {
            UserId = userId;
        }
    }
}