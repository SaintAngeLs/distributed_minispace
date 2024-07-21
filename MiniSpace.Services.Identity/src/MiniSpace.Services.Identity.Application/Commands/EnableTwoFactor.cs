using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class EnableTwoFactor : ICommand
    {
        public Guid UserId { get; }
        public string Secret { get; }

        public EnableTwoFactor(Guid userId, string secret)
        {
            UserId = userId;
            Secret = secret;
        }
    }
}
