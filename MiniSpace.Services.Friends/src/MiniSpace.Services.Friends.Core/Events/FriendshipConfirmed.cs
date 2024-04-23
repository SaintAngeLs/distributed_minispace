namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipConfirmed : IDomainEvent
    {
        public Guid FriendId { get; }
        
        public FriendshipConfirmed(Guid friendId)
        {
            FriendId = friendId;
        }
    }
}
