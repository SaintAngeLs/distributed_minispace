using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationPostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<PostDocument> OrganizationPosts { get; set; }
    }
}
