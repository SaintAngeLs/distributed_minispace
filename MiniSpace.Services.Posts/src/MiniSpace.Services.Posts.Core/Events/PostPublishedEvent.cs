namespace MiniSpace.Services.Posts.Core.Events
{
    public class PostPublishedEvent : IDomainEvent
    {
        public Guid PostId { get; }
        public DateTime OccurredOn { get; }

        public PostPublishedEvent(Guid postId)
        {
            PostId = postId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
