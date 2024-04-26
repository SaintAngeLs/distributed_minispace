using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class FriendRequest : AggregateRoot
    {
        public Guid InviterId { get; private set; }
        public Guid InviteeId { get; private set; }
        public DateTime RequestedAt { get; private set; }

        public FriendRequest(Guid inviterId, Guid inviteeId, DateTime requestedAt)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
            RequestedAt = requestedAt;
            AddEvent(new FriendRequestCreated(this)); 
        }
    }
}