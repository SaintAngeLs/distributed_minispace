using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
        Task PublishAsync(IEnumerable<IEvent> events);
    }    
}
