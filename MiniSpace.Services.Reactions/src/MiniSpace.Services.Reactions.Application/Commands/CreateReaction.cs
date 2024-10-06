using System;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class CreateReaction : ICommand
    {
        public Guid ReactionId { get; }
        public Guid UserId { get; }
        public string ReactionType { get; }
        public Guid ContentId { get; }
        public string ContentType { get; }
        public string TargetType { get; }

        public CreateReaction(Guid reactionId, Guid userId, Guid contentId, string reactionType, string contentType, string targetType)
        {
            ReactionId = reactionId == Guid.Empty ? Guid.NewGuid() : reactionId;
            UserId = userId;
            ContentId = contentId;
            ReactionType = reactionType;
            ContentType = contentType;
            TargetType = targetType;
        }
    }    
}
