using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRemoved : IDomainEvent
    {
        public User Requester { get; private set; }
        public User Friend { get; private set; }

        public FriendRemoved(User requester, User friend)
        {
            Requester = requester;
            Friend = friend;
        }
    }
}
