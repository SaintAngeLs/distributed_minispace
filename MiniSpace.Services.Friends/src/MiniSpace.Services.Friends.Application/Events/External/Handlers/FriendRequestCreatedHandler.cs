using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
{
    public class FriendRequestCreatedHandler : IEventHandler<FriendRequestCreated>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendRequestCreatedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendRequestCreated @event, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var request = new FriendRequest(@event.RequesterId, @event.FriendId, now);
            await _friendRepository.AddRequestAsync(request);

            var events = _eventMapper.MapAll(request.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}