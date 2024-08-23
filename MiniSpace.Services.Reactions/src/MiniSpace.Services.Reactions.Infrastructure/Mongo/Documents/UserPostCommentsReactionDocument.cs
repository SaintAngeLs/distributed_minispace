using System;
using System.Collections.Generic;
using Convey.Types;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class UserPostCommentsReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserPostCommentId { get; set; }
        public Guid UserId { get; set; }
        public List<ReactionDocument> Reactions { get; set; } = new List<ReactionDocument>();
    }
}
