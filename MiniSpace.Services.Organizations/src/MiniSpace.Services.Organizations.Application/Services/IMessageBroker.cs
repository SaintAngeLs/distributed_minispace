using Paralax.CQRS.Events;

namespace MiniSpace.Services.Organizations.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
        Task PublishAsync(IEnumerable<IEvent> events);
    }    
}
