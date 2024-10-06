using Paralax.CQRS.Events;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core;
using MiniSpace.Services.Comments.Core.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Services
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
