using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Identity.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class TokenNotFoundException : AppException
    {
        public override string Code { get; } = "token_not_found";
        public Guid UserId { get; }
        
        public TokenNotFoundException(Guid userId) 
            : base($"No valid reset token found for user ID: '{userId}'.")
        {
            UserId = userId;
        }
    }
}
