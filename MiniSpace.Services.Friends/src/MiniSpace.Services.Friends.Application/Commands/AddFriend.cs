using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class AddFriend : ICommand
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public AddFriend(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
