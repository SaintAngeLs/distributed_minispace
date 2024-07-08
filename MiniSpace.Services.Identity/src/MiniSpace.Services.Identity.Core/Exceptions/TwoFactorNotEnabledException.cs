using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class TwoFactorNotEnabledException : DomainException
    {
        public override string Code { get; } = "two_factor_not_enabled";
        
        public TwoFactorNotEnabledException(Guid userId) 
            : base($"Two-factor authentication is not enabled for user with ID: {userId}.")
        {
        }
    }
}
