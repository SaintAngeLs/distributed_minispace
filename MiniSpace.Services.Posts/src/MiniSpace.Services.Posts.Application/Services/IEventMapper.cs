using Convey.CQRS.Events;
using MiniSpace.Services.Posts.Core.Events;

namespace MiniSpace.Services.Posts.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}
