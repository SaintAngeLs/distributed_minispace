namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipDeclined : IDomainEvent
    {
        public Guid FriendshipId { get; }

        public FriendshipDeclined(Guid friendshipId)
        {
            FriendshipId = friendshipId;
        }
    }
}
