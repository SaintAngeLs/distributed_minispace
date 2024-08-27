using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Commands;
using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        IEnumerable<IEvent> MapAll(PendingFriendAccepted pendingFriendAccept);
    }    
}
