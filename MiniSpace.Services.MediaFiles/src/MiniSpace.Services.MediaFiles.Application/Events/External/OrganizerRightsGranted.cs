using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
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
