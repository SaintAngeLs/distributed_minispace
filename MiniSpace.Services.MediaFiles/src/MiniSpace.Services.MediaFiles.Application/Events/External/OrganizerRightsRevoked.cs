using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
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
