using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestCreated : IDomainEvent
    {
        public FriendRequest FriendRequest { get; private set; }

        public FriendRequestCreated(FriendRequest friendRequest)
        {
            FriendRequest = friendRequest;
        }
    }
}
