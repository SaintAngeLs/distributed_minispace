using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class GenerateTwoFactorSecret : ICommand
    {
        public Guid UserId { get; }

        public GenerateTwoFactorSecret(Guid userId)
        {
            UserId = userId;
        }
    }
}
