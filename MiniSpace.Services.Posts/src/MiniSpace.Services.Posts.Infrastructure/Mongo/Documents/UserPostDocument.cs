using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserPostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<PostDocument> UserPosts { get; set; }
    }
}
