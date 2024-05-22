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
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext; 

        public RemoveFriendHandler(IStudentFriendsRepository studentFriendsRepository, IMessageBroker messageBroker, IEventMapper eventMapper, IAppContext appContext)
        {
            _studentFriendsRepository = studentFriendsRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext; 
        }

        public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            Console.WriteLine($"Handling RemoveFriend for RequesterId: {command.RequesterId} and FriendId: {command.FriendId}. Authenticated: {identity.IsAuthenticated}");

            // Check if the friendship exists for both directions
            var requesterFriends = await _studentFriendsRepository.GetAsync(command.RequesterId);
            var friendFriends = await _studentFriendsRepository.GetAsync(command.FriendId);

            if (!requesterFriends.Friends.Any(f => f.FriendId == command.FriendId) || !friendFriends.Friends.Any(f => f.FriendId == command.RequesterId))
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

            // Remove the friendship in both directions
            requesterFriends.RemoveFriend(command.FriendId);
            friendFriends.RemoveFriend(command.RequesterId);

            // Save the updates to both friend records
            await _studentFriendsRepository.UpdateAsync(requesterFriends);
            await _studentFriendsRepository.UpdateAsync(friendFriends);

            // Publish an event indicating the friend has been removed
            var eventToPublish = new PendingFriendDeclined(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(eventToPublish);

            // Publish a reciprocal event for the inverse relationship
            var reciprocalEventToPublish = new PendingFriendDeclined(command.FriendId, command.RequesterId);
            await _messageBroker.PublishAsync(reciprocalEventToPublish);
        }
    }
}
