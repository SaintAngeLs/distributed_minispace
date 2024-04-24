using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
{
    public class FriendInvitedHandler : IEventHandler<FriendInvited>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendInvitedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendInvited @event, CancellationToken cancellationToken)
        {
            var request = new FriendRequest(@event.InviterId, @event.InviteeId);
            await _friendRepository.AddInvitationAsync(request);  

            var events = _eventMapper.MapAll(request.Events); 
            await _messageBroker.PublishAsync(events.ToArray());
        }

    }
}