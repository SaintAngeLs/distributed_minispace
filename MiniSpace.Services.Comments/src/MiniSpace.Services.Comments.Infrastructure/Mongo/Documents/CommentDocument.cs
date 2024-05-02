using Convey.Types;
using MiniSpace.Services.Comments.Core.Entities;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public class CommentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Guid StudentId { get; private set; }
        public List<Guid> Likes { get; private set; }
        public Guid ParentId { get; private set; }
        public string Comment { get; private set; }
        public DateTime PublishDate { get; private set; }
    }    
}
