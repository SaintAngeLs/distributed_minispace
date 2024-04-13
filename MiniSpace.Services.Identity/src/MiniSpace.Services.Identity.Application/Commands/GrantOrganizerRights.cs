using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class GrantOrganizerRights(Guid userId) : ICommand
    {
        public Guid UserId { get; } = userId;
    }
}