using Convey.CQRS.Events;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core;
using MiniSpace.Services.MediaFiles.Core.Events;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                
            }

            return null;
        }
    }
}
