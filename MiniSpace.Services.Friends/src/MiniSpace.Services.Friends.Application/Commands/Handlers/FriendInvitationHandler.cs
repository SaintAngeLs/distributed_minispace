using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class FriendInvitationHandler : ICommandHandler<FriendInvited>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;

        public FriendInvitationHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext;
        }

        public async Task HandleAsync(FriendInvited command, CancellationToken cancellationToken = default)
        {
            // Check identity context for authorization, etc.
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || identity.Id != command.InviterId)
            {
                throw new UnauthorizedAccessException("Not authorized to send friend invitations.");
            }

            // Logic to send an invitation
            await _friendRepository.InviteFriendAsync(command.InviterId, command.InviteeId);

            // Trigger events
            var eventToPublish = _eventMapper.Map(new FriendInvited(command.InviterId, command.InviteeId));
            await _messageBroker.PublishAsync(eventToPublish);
        }
    }
}
