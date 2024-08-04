using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class VerifyTwoFactorCode : ICommand
    {
        public Guid UserId { get; }
        public string Code { get; }

        public VerifyTwoFactorCode(Guid userId, string code)
        {
            UserId = userId;
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
