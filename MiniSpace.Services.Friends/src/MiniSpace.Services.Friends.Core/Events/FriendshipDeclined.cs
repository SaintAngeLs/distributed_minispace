namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipDeclined : IDomainEvent
    {
        // Adding separate properties for the requester and the friend
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public FriendshipDeclined(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
