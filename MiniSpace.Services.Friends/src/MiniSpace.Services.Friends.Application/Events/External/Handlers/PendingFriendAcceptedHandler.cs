using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
{
    public class PendingFriendAcceptedHandler : IEventHandler<PendingFriendAccepted>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public PendingFriendAcceptedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(PendingFriendAccepted @event, CancellationToken cancellationToken)
        {
            var friendship = await _friendRepository.GetFriendshipAsync(@event.RequesterId, @event.FriendId);
            if (friendship == null)
            {
                throw new FriendshipNotFoundException(@event.RequesterId, @event.FriendId);
            }

            friendship.MarkAsConfirmed();
            await _friendRepository.UpdateFriendshipAsync(friendship);

            if (await _friendRepository.GetFriendshipAsync(@event.FriendId, @event.RequesterId) == null)
            {
                var reciprocalFriendship = new Core.Entities.Friend(@event.FriendId, @event.RequesterId, DateTime.UtcNow, Core.Entities.FriendState.Accepted);
                await _friendRepository.AddAsync(reciprocalFriendship);
                reciprocalFriendship.MarkAsConfirmed();
                await _friendRepository.UpdateFriendshipAsync(reciprocalFriendship);
            }

            // Publish the confirmation event
            var confirmationEvent = new FriendshipConfirmed(@event.RequesterId, @event.FriendId);
            await _messageBroker.PublishAsync(confirmationEvent);
        }
    }
}