using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Reactions.Application.Commands
{
    public class UpdateReaction : ICommand
    {
        public Guid ReactionId { get; }
        public Guid UserId { get; }
        public string NewReactionType { get; }
        public string ContentType { get; }
        public string TargetType { get; }

        public UpdateReaction(Guid reactionId, Guid userId, string newReactionType, string contentType, string targetType)
        {
            ReactionId = reactionId;
            UserId = userId;
            NewReactionType = newReactionType;
            ContentType = contentType;
            TargetType = targetType;
        }
    }
}
