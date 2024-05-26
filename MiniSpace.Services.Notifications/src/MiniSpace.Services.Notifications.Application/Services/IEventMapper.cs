using Convey.CQRS.Events;
using MiniSpace.Services.Notifications.Core.Events;

namespace MiniSpace.Services.Notifications.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
