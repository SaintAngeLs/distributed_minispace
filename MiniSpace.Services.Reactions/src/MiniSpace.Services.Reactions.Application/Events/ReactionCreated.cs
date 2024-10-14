using Paralax.CQRS.Events;
using System;

namespace MiniSpace.Services.Reactions.Application.Events
{
    public class ReactionCreated : IEvent
    {
        public Guid ReactionId { get; }
        public Guid UserId { get; }
        public Guid ContentId { get; }
        public string ContentType { get; }
        public string ReactionType { get; }
        public string TargetType { get; }

        public ReactionCreated(Guid reactionId, Guid userId, Guid contentId, string contentType, string reactionType, string targetType)
        {
            ReactionId = reactionId;
            UserId = userId;
            ContentId = contentId;
            ContentType = contentType;
            ReactionType = reactionType;
            TargetType = targetType;
        }
    }
}
