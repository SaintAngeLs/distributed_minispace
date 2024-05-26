using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestRemoved : IDomainEvent
    {
        public FriendRequest FriendRequest { get; private set; }

        public FriendRequestRemoved(FriendRequest friendRequest)
        {
            FriendRequest = friendRequest;
        }
    }
}
