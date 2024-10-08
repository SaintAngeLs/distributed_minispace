using System;
using System.Collections.Generic;
using MiniSpace.Services.Reactions.Core.Events;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Reaction : AggregateRoot
    {
        public Guid UserId { get; private set; }  
        public ReactionType ReactionType { get; private set; }
        public Guid ContentId { get; private set; }
        public ReactionContentType ContentType { get; private set; }
        public ReactionTargetType TargetType { get; private set; }

        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private Reaction(Guid reactionId, Guid userId, ReactionType reactionType, Guid contentId, 
                         ReactionContentType contentType, ReactionTargetType targetType) 
        {
            Id = reactionId;
            UserId = userId;
            ReactionType = reactionType;
            ContentId = contentId;
            ContentType = contentType;
            TargetType = targetType;

            AddDomainEvent(new ReactionCreatedEvent(reactionId, userId, reactionType, contentId, contentType, targetType));
        }

        public static Reaction Create(Guid reactionId, Guid userId, ReactionType reactionType, Guid contentId, 
                                        ReactionContentType contentType, ReactionTargetType targetType)
        {
            return new Reaction(reactionId, userId, reactionType, contentId, contentType, targetType);
        }

        public void UpdateReactionType(ReactionType newReactionType)
        {
            if (ReactionType != newReactionType)
            {
                ReactionType = newReactionType;
                AddDomainEvent(new ReactionUpdatedEvent(Id, newReactionType));
            }
        }
    }
}
