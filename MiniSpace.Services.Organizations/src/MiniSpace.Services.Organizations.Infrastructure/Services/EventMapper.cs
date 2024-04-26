using Convey.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Infrastructure.Services
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
