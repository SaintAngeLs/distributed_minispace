using Paralax.CQRS.Events;
using MiniSpace.Services.Communication.Core.Events;

namespace MiniSpace.Services.Communication.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
