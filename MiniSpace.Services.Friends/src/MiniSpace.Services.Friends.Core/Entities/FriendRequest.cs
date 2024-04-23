using MiniSpace.Services.Friends.Core.Events;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class FriendRequest
    {
        public Guid Id { get; private set; }
        public Guid InviterId { get; private set; }
        public Guid InviteeId { get; private set; }
        public DateTime RequestedAt { get; private set; }

        private List<IDomainEvent> _events = new List<IDomainEvent>();
        public void AddEvent(IDomainEvent eventItem)
        {
            _events.Add(eventItem);
        }
        public IReadOnlyList<IDomainEvent> Events => _events.AsReadOnly();

        public FriendRequest(Guid inviterId, Guid inviteeId)
        {
            Id = Guid.NewGuid();
            InviterId = inviterId;
            InviteeId = inviteeId;
            RequestedAt = DateTime.UtcNow;
        }
    }
}
