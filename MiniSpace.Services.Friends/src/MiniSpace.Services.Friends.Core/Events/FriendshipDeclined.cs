using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
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
