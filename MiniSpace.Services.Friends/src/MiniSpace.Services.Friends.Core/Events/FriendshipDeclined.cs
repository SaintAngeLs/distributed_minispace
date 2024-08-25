namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipDeclined : IDomainEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public FriendshipDeclined(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
