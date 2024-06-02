using Convey.CQRS.Events;
using MiniSpace.Services.Email.Core.Events;

namespace MiniSpace.Services.Email.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
