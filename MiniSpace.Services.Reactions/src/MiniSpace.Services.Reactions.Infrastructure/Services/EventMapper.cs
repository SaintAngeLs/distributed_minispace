using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Events;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core;
using MiniSpace.Services.Reactions.Core.Events;

namespace MiniSpace.Services.Reactions.Infrastructure.Services
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
