using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class BlockedUsersDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public IEnumerable<BlockedUserEntry> BlockedUsers { get; set; } = new List<BlockedUserEntry>();

        public class BlockedUserEntry
        {
            public Guid BlockedUserId { get; set; }
            public DateTime BlockedAt { get; set; }
        }
    }
}
