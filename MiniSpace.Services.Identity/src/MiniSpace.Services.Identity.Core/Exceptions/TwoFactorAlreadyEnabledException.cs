using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class TwoFactorAlreadyEnabledException : DomainException
    {
        public override string Code { get; } = "two_factor_already_enabled";
        
        public TwoFactorAlreadyEnabledException(Guid userId) 
            : base($"Two-factor authentication is already enabled for user with ID: {userId}.")
        {
        }
    }

}
