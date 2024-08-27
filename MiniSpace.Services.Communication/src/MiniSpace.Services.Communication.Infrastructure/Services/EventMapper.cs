using Convey.CQRS.Events;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Core.Events;

namespace MiniSpace.Services.Communication.Infrastructure.Services
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
