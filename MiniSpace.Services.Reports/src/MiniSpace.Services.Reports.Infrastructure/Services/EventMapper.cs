using Paralax.CQRS.Events;
using MiniSpace.Services.Reports.Application.Services;
using MiniSpace.Services.Reports.Core;
using MiniSpace.Services.Reports.Core.Events;

namespace MiniSpace.Services.Reports.Infrastructure.Services
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
