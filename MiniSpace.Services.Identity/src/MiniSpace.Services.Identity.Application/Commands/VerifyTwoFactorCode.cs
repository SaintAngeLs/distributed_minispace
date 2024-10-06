using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Identity.Application.Commands
{
    public class VerifyTwoFactorCode : ICommand
    {
        public Guid UserId { get; }
        public string Code { get; }
        public string DeviceType { get; }

        public VerifyTwoFactorCode(Guid userId, string code, string deviceType)
        {
            UserId = userId;
            Code = code ?? throw new ArgumentNullException(nameof(code));
            DeviceType = deviceType ?? throw new ArgumentNullException(nameof(deviceType));
        }
    }
}
