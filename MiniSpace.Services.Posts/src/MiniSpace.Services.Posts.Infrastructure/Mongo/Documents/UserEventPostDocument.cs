using System.Diagnostics.CodeAnalysis;
using Paralax.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserEventPostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<PostDocument> UserEventPosts { get; set; }
    }
}
