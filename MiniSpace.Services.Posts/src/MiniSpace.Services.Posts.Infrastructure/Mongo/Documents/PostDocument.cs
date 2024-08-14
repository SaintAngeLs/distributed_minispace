using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using MiniSpace.Services.Posts.Core.Entities;
using System;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class PostDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid? EventId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; } 
        public string TextContent { get; set; }
        public IEnumerable<string> MediaFiles { get; set; }
        public State State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public PostContext Context { get; set; } 
    }
}
