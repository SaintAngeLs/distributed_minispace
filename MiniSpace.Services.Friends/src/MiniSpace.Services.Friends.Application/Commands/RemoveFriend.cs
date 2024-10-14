using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class RemoveFriend : ICommand
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public RemoveFriend(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
