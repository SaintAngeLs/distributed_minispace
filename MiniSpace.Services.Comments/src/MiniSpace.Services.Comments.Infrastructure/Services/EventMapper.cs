using Convey.CQRS.Events;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core;
using MiniSpace.Services.Comments.Core.Events;

namespace MiniSpace.Services.Comments.Infrastructure.Services
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
