using Paralax.CQRS.Events;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core;
using MiniSpace.Services.Organizations.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
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
