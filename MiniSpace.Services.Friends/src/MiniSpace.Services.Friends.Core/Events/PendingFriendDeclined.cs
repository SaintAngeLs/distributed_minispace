using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class PendingFriendDeclined : IDomainEvent
    {
        public Friend Requester { get; }
        public Friend Decliner { get; }

        public PendingFriendDeclined(Friend requester, Friend decliner)
        {
            Requester = requester;
            Decliner = decliner;
        }
    }

}
