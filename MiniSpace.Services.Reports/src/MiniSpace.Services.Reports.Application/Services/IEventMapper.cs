using Convey.CQRS.Events;
using MiniSpace.Services.Reports.Core.Events;

namespace MiniSpace.Services.Reports.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
