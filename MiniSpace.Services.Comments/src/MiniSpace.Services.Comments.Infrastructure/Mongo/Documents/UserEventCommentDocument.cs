using Paralax.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserEventCommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserEventId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<CommentDocument> Comments { get; set; } = new List<CommentDocument>();
    }
}
