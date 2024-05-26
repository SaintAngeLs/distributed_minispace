namespace MiniSpace.Services.Friends.Core.Events
{
    public class PendingFriendAccepted :  IDomainEvent 
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendAccepted(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
