using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class DisableTwoFactor : ICommand
    {
        public Guid UserId { get; }

        public DisableTwoFactor(Guid userId)
        {
            UserId = userId;
        }
    }
}
