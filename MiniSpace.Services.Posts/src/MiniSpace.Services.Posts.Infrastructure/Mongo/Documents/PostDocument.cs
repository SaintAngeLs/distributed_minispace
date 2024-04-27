using Convey.Types;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class PostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid OrganizerId { get; set; }
        public string TextContent { get; set; }
        public string MediaContent { get; set; }
        public State State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }    
}
