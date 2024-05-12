using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class FriendRequestSentHandler : IEventHandler<FriendRequestSent>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public FriendRequestSentHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(FriendRequestSent @event, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var request = new FriendRequest(
                inviterId: @event.InviterId, 
                inviteeId: @event.InviteeId, 
                requestedAt: now, 
                state: FriendState.Requested 
            );
            await _friendRepository.AddRequestAsync(request);
            
            var events = _eventMapper.MapAll(request.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}