using Convey.Types;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public class CommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get;  set; }
        public Guid ContextId { get;  set; }
        public CommentContext CommentContext { get; set; }
        public Guid StudentId { get;  set; }
        public List<Guid> Likes { get;  set; }
        public Guid ParentId { get;  set; }
        public string TextContent { get;  set; }
        public DateTime CreatedAt { get;  set; }
    }    
}
