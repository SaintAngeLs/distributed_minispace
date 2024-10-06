using System.Threading.Tasks;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}