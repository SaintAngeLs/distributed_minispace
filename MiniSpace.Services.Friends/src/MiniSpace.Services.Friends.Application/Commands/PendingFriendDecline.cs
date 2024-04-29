using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class PendingFriendDecline : ICommand
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendDecline(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
