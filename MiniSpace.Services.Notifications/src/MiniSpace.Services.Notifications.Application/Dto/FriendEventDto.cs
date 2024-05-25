namespace MiniSpace.Services.Notifications.Application.Dto
{
    public class FriendEventDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string EventType { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
