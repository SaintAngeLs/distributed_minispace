using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Identity.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}