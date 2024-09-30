using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Paralax.CQRS.Events;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            // TODO: update mapper
            switch (@event)
            {
                
            }

            return null;
        }
    }
}