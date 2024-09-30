using Paralax.CQRS.Events;
using MiniSpace.Services.Students.Core.Events;

namespace MiniSpace.Services.Students.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
