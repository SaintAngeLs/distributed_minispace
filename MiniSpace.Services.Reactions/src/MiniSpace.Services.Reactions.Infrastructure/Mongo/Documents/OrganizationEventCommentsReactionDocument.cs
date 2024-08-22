using System;
using System.Collections.Generic;
using Convey.Types;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class OrganizationEventCommentsReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationEventCommentId { get; set; }
        public Guid OrganizationId { get; set; }
        public List<ReactionDocument> Reactions { get; set; } = new List<ReactionDocument>();
    }
}