using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Infrastructure.Services
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
