using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class BanUser(Guid userId) : ICommand
    {
        public Guid UserId { get; } = userId;
    }
}