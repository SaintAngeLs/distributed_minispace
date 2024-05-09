using Convey.CQRS.Events;
using MiniSpace.Services.Reactions.Core.Events;

namespace MiniSpace.Services.Reactions.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}
