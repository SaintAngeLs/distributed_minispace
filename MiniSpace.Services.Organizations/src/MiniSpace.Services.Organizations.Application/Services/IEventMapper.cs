using Paralax.CQRS.Events;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
