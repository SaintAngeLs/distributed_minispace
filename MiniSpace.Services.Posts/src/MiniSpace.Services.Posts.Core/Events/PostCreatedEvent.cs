namespace MiniSpace.Services.Posts.Core.Events
{
    public class PostCreatedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public DateTime OccurredOn { get; }

        public PostCreatedEvent(Guid postId)
        {
            PostId = postId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
