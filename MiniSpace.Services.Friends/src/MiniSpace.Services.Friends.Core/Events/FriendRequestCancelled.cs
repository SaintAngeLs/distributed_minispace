using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestCancelled : IDomainEvent
    {
        public Guid InviterId { get; private set; }
        public Guid InviteeId { get; private set; }

        public FriendRequestCancelled(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
