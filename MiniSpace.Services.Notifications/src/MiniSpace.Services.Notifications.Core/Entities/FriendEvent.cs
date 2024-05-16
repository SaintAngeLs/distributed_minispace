namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class FriendEvent
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid FriendId { get; private set;}
        public string EventType { get; private set; }
        public string Details { get; private set; }
        public DateTime CreatedAt { get; private set; }

          public FriendEvent(Guid id, Guid eventId, Guid userId, Guid friendId, string eventType, string details, DateTime createdAt)
        {
            Id = id;
            EventId = eventId;
            UserId = userId;
            FriendId = friendId;
            EventType = eventType;
            Details = details;
            CreatedAt = createdAt;
        }
    }
}
