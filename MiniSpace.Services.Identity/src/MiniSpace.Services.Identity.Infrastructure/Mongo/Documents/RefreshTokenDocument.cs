using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.Types;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    internal sealed  class RefreshTokenDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}