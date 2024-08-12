using System;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Events
{
    public class ReactionCreatedEvent : IDomainEvent
    {
        public Guid ReactionId { get; }
        public Guid StudentId { get; }
        public string StudentFullName { get; }
        public ReactionType ReactionType { get; }
        public Guid ContentId { get; }
        public ReactionContentType ContentType { get; }
        public ReactionTargetType TargetType { get; }

        public ReactionCreatedEvent(Guid reactionId, Guid studentId, string studentFullName, ReactionType reactionType,
            Guid contentId, ReactionContentType contentType, ReactionTargetType targetType)
        {
            ReactionId = reactionId;
            StudentId = studentId;
            StudentFullName = studentFullName;
            ReactionType = reactionType;
            ContentId = contentId;
            ContentType = contentType;
            TargetType = targetType;
        }
    }
    
    public class ReactionUpdatedEvent : IDomainEvent
    {
        public Guid ReactionId { get; }
        public ReactionType NewReactionType { get; }
        
        public ReactionUpdatedEvent(Guid reactionId, ReactionType newReactionType)
        {
            ReactionId = reactionId;
            NewReactionType = newReactionType;
        }
    }
}
