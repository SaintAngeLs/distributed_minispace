using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
{
    public class PendingFriendDeclinedHandler : IEventHandler<PendingFriendDeclined>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public PendingFriendDeclinedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(PendingFriendDeclined @event, CancellationToken cancellationToken)
        {
            var friendship = await _friendRepository.GetFriendshipAsync(@event.RequesterId, @event.FriendId);
            if (friendship is null)
            {
                throw new FriendshipNotFoundException(@event.RequesterId, @event.FriendId);
            }

            friendship.MarkAsDeclined();
            await _friendRepository.UpdateAsync(friendship);

            var events = _eventMapper.MapAll(friendship.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}