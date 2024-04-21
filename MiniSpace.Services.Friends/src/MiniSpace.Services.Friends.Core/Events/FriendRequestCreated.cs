using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestCreated : IDomainEvent
    {
        public Friend Requester { get; }
        public Friend Requestee { get; }

        public FriendRequestCreated(Friend requester, Friend requestee)
        {
            Requester = requester;
            Requestee = requestee;
        }
    }
}
