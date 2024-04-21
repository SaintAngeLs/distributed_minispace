using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class PendingFriendAccepted : IDomainEvent
    {
        public Friend Requester { get; }
        public Friend Acceptor { get; }

        public PendingFriendAccepted(Friend requester, Friend acceptor)
        {
            Requester = requester;
            Acceptor = acceptor;
        }
    }
}
