using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

/*
  This will require an update:
    - RemoveFriendHandler.cs should be responsible for handling the RemoveFriend command with 
    that in mind that it will remove a friend from the user's friend list but should leave the 
    removed frined requests as avaising lite to accept (make the user subscriber).
*/
namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class RemoveFriendHandler : ICommandHandler<RemoveFriend>
    {
        private readonly IUserFriendsRepository _userFriendsRepository;
        private readonly IUserRequestsRepository _userRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext; 

          public RemoveFriendHandler(
            IUserFriendsRepository userFriendsRepository, 
            IUserRequestsRepository userRequestsRepository,
            IMessageBroker messageBroker, 
            IEventMapper eventMapper, 
            IAppContext appContext)
        {
            _userFriendsRepository = userFriendsRepository;
            _userRequestsRepository = userRequestsRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext; 
        }

         public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            Console.WriteLine($"Handling RemoveFriend for RequesterId: {command.RequesterId} and FriendId: {command.FriendId}. Authenticated: {identity.IsAuthenticated}");

            var requesterFriends = await _userFriendsRepository.GetAsync(command.RequesterId);
            var friendFriends = await _userRequestsRepository.GetAsync(command.FriendId);

            if (requesterFriends == null || friendFriends == null)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

            await _userFriendsRepository.RemoveFriendAsync(command.RequesterId, command.FriendId);
            await _userFriendsRepository.RemoveFriendAsync(command.FriendId, command.RequesterId);

            await _userRequestsRepository.RemoveFriendRequestAsync(command.RequesterId, command.FriendId);
            await _userRequestsRepository.RemoveFriendRequestAsync(command.FriendId, command.RequesterId);

            var eventToPublish = new PendingFriendDeclined(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(eventToPublish);
            var reciprocalEventToPublish = new PendingFriendDeclined(command.FriendId, command.RequesterId);
            await _messageBroker.PublishAsync(reciprocalEventToPublish);
        }

    }
}
