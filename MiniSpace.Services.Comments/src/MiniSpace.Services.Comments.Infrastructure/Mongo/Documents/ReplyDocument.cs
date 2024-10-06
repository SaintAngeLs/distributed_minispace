using Paralax.Types;
using MiniSpace.Services.Comments.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class ReplyDocument
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
