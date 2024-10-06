using Paralax.CQRS.Events;
using MiniSpace.Services.MediaFiles.Core.Events;

namespace MiniSpace.Services.MediaFiles.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }    
}
