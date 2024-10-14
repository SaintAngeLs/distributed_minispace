using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class PendingFriendAccept : ICommand
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendAccept(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
