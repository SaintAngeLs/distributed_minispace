using Convey.CQRS.Events;
using MiniSpace.Services.Students.Core;

namespace MiniSpace.Services.Students.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
