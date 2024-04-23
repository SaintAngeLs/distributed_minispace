using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class FriendAddedHandler : IEventHandler<FriendAdded>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendAddedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendAdded @event, CancellationToken cancellationToken)
        {
            var friendship = await _friendRepository.GetFriendshipAsync(@event.RequesterId, @event.FriendId);
            if (friendship is null)
            {
                throw new FriendshipNotFoundException(@event.RequesterId, @event.FriendId);
            }

            friendship.MarkAsConfirmed();
            await _friendRepository.UpdateAsync(friendship);

            var events = _eventMapper.MapAll(friendship.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}
