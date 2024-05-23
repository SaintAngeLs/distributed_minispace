using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class RemoveFriendHandler : ICommandHandler<RemoveFriend>
    {
        private readonly IStudentFriendsRepository _studentFriendsRepository;
        private readonly IStudentRequestsRepository _studentRequestsRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext; 

          public RemoveFriendHandler(
            IStudentFriendsRepository studentFriendsRepository, 
            IStudentRequestsRepository studentRequestsRepository,
            IMessageBroker messageBroker, 
            IEventMapper eventMapper, 
            IAppContext appContext)
        {
            _studentFriendsRepository = studentFriendsRepository;
            _studentRequestsRepository = studentRequestsRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext; 
        }

         public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            Console.WriteLine($"Handling RemoveFriend for RequesterId: {command.RequesterId} and FriendId: {command.FriendId}. Authenticated: {identity.IsAuthenticated}");

            var requesterFriends = await _studentFriendsRepository.GetAsync(command.RequesterId);
            var friendFriends = await _studentFriendsRepository.GetAsync(command.FriendId);

            if (requesterFriends == null || friendFriends == null)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

            // Call specific methods to remove the friend connection
            await _studentFriendsRepository.RemoveFriendAsync(command.RequesterId, command.FriendId);
            await _studentFriendsRepository.RemoveFriendAsync(command.FriendId, command.RequesterId);

            // Remove the corresponding friend requests
            await _studentRequestsRepository.RemoveFriendRequestAsync(command.RequesterId, command.FriendId);
            await _studentRequestsRepository.RemoveFriendRequestAsync(command.FriendId, command.RequesterId);

            // Publish events indicating the removal of pending friend requests
            var eventToPublish = new PendingFriendDeclined(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(eventToPublish);
            var reciprocalEventToPublish = new PendingFriendDeclined(command.FriendId, command.RequesterId);
            await _messageBroker.PublishAsync(reciprocalEventToPublish);
        }

    }
}
