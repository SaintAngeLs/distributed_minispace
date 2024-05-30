using System;
using System.Diagnostics.CodeAnalysis;
using Convey.Types;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    internal sealed class UserResetTokenDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }  
        public Guid UserId { get; set; } 
        public string ResetToken { get; set; } 
        public DateTime? ResetTokenExpires { get; set; } 
    }
}
