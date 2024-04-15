using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class UnbanUser(Guid userId) : ICommand
    {
        public Guid UserId { get; } = userId;
    }
}