using Paralax.CQRS.Events;
using Paralax.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("events")]
    public class UserCancelledInterestInEvent : IEvent
    {
        public Guid EventId { get; }
        public Guid UserId { get; }

        public UserCancelledInterestInEvent(Guid eventId, Guid userId)
        {
            EventId = eventId;
            UserId = userId;
        }
    } 
}