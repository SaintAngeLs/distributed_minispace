using System;
using System.Collections.Generic;
using Paralax.Types;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class OrganizationPostReactionDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; } 
        public Guid OrganizationPostId { get; set; }
        public Guid OrganizationId { get; set; } 
        public List<ReactionDocument> Reactions { get; set; } = new List<ReactionDocument>();
    }
}
