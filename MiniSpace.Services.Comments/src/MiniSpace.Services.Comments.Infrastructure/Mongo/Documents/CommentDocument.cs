using Convey.Types;
using MiniSpace.Services.Comments.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class CommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get;  set; }
        public Guid ContextId { get;  set; }
        public CommentContext CommentContext { get; set; }
        public Guid StudentId { get;  set; }
        public string StudentName { get;  set; }
        public IEnumerable<Guid> Likes { get;  set; }
        public Guid ParentId { get;  set; }
        public string TextContent { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime LastReplyAt { get;  set; }
        public int RepliesCount { get;  set; }
        public bool IsDeleted { get; set; }
    }    
}
