using Convey.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class UserPostCommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<CommentDocument> Comments { get; set; } = new List<CommentDocument>();
    }
}
