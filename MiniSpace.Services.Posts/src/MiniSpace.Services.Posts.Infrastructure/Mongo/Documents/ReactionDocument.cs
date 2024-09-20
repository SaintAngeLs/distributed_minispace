using System;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    public class ReactionDocument
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public string ContentType { get; set; }
        public string ReactionType { get; set; }
        public string TargetType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
