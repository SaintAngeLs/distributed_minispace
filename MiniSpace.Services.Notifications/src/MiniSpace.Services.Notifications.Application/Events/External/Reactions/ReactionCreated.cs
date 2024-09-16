using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Reactions
{
    [Message("reactions")]
    public class ReactionCreated : IEvent
    {
        public Guid ReactionId { get; }
        public Guid UserId { get; }
        public Guid ContentId { get; }
        public string ReactionType { get; }
        public string ContentType { get; }
        public string TargetType { get; }

        public ReactionCreated(Guid reactionId, Guid userId, Guid contentId, string reactionType, string contentType, string targetType)
        {
            ReactionId = reactionId;
            UserId = userId;
            ContentId = contentId;
            ReactionType = reactionType;
            ContentType = contentType;
            TargetType = targetType;
        }
    }
}
