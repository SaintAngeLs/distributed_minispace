using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class FriendRequest : AggregateRoot
    {
        public Guid InviterId { get; private set; }
        public Guid InviteeId { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public FriendState _state;
        public FriendState State
        {
            get => _state;
            set => _state = value;
        }

        public FriendRequest(Guid inviterId, Guid inviteeId, DateTime requestedAt, FriendState state)
        {
            Id = Guid.NewGuid();
            InviterId = inviterId;
            InviteeId = inviteeId;
            RequestedAt = requestedAt;
            _state = state;
        }

        public void Accept()
        {
            if (State != FriendState.Requested)
                throw new InvalidOperationException("Only requested friend requests can be accepted.");

            _state  = FriendState.Accepted;
            AddEvent(new FriendshipConfirmed(Id));
        }

        public void Decline()
        {
            if (State != FriendState.Requested)
                throw new InvalidOperationException("Only requested friend requests can be declined.");

            _state = FriendState.Declined;
            AddEvent(new FriendshipDeclined(InviterId, InviteeId));
        }

    }
}
