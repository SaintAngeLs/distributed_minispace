using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationEventPostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<PostDocument> EventPosts { get; set; }
    }
}
