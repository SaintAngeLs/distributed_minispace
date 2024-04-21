using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Students.Application.Events.External
{
    [Message("identity")]
    public class OrganizerRightsGranted : IEvent
    {
        public Guid UserId { get; }
        public OrganizerRightsGranted(Guid userId)
        {
            UserId = userId;
        }
    }
}
