using System;
using System.Collections.Generic;
using Convey.Types;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class UserPostReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public List<ReactionDocument> Reactions { get; set; } = new List<ReactionDocument>();

    }
}
