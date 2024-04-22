using Convey.CQRS.Events;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Events.External.Handlers
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
            var invitation = new FriendInvitation(@event.InviterId, @event.InviteeId);
            await _friendRepository.AddInvitationAsync(invitation);

            var events = _eventMapper.MapAll(invitation.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }

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
            var request = new FriendRequest(@event.RequesterId, @event.FriendId);
            await _friendRepository.AddRequestAsync(request);

            var events = _eventMapper.MapAll(request.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }

    public class FriendRequestSentHandler : IEventHandler<FriendRequestSent>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public FriendRequestSentHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(FriendRequestSent @event, CancellationToken cancellationToken)
        {
            var request = new FriendRequest(@event.InviterId, @event.InviteeId);
            await _friendRepository.AddRequestAsync(request);

            var events = _eventMapper.MapAll(request.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }

    public class PendingFriendAcceptedHandler : IEventHandler<PendingFriendAccepted>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public PendingFriendAcceptedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(PendingFriendAccepted @event, CancellationToken cancellationToken)
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

    public class PendingFriendDeclinedHandler : IEventHandler<PendingFriendDeclined>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public PendingFriendDeclinedHandler(IFriendRepository friendRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _friendRepository = friendRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
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
