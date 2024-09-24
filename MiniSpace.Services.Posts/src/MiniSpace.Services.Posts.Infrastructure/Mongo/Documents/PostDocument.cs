using System.Diagnostics.CodeAnalysis;
using Convey.Types;
using MiniSpace.Services.Posts.Core.Entities;
using System;
using System.Collections.Generic;

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
        public VisibilityStatus Visibility { get; set; }
        public PostType Type { get; set; }
        public string Title { get; set; }

        // New fields to track page ownership
        public Guid? PageOwnerId { get; set; } // Owner of the page where the post is published (User or Organization)
        public PageOwnerType PageOwnerType { get; set; } // Specifies if the page belongs to a user or an organization

        // New field to track reposts
        public Guid? OriginalPostId { get; set; } // Reference to the original post if this is a repost
        public bool IsRepost => OriginalPostId.HasValue; // Indicates if the post is a repost
    }
}
