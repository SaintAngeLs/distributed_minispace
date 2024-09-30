using Paralax.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationPostCommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationPostId { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<CommentDocument> Comments { get; set; } = new List<CommentDocument>();
    }
}
