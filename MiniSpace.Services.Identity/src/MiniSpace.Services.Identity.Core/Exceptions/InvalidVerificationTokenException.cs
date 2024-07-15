using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class InvalidVerificationTokenException : DomainException
    {
        public override string Code { get; } = "invalid_verification_token";
        
        public InvalidVerificationTokenException() : base("Invalid or expired verification token.")
        {
        }
    }

}