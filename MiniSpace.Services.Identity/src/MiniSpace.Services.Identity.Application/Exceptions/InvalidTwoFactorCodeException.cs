using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Identity.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidTwoFactorCodeException : AppException
    {
        public override string Code { get; } = "invalid_two_factor_code";
        
        public InvalidTwoFactorCodeException() 
            : base("Invalid or incorrect two-factor authentication code provided.")
        {
        }
    }
}
