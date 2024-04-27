using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.Events.External
{
    [Message("identity") ]
    public class OrganizerRightsGranted : IEvent
    {
        public Guid UserId { get; }
        
        public OrganizerRightsGranted(Guid userId)
        {
            UserId = userId;
        }
    }
}