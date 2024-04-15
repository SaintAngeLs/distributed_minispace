using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class OrganizerRightsRevoked : IEvent
    {
        public Guid UserId { get; }
        public OrganizerRightsRevoked(Guid userId)
        {
            UserId = userId;
        }
    }
}
