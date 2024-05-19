using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.MediaFiles.Application.Events.External
{
    [Message("events")]
    public class EventUpdated : IEvent
    {
        public Guid EventId { get; }
        public IEnumerable<Guid> MediaFilesIds { get; }

        public EventUpdated(Guid eventId, IEnumerable<Guid> mediaFilesIds)
        {
            EventId = eventId;
            MediaFilesIds = mediaFilesIds;
        }
    }  
}