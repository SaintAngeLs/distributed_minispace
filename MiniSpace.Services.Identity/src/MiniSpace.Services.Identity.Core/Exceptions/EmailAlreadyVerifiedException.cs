using System;

namespace MiniSpace.Services.Identity.Core.Exceptions
{
    public class EmailAlreadyVerifiedException : DomainException
    {
        public override string Code { get; } = "email_already_verified";
        
        public EmailAlreadyVerifiedException() : base("Email is already verified.")
        {
        }
    }
}