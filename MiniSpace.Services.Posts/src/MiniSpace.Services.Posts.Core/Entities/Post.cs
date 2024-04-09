namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Post : AggregateRoot
    {
        public Guid EventId { get; private set; }
        public Guid StudentId { get; private set; }
        public string TextContent { get; private set; }
        public string MediaContent { get; private set; }

        public Post(Guid id, Guid eventId, Guid studentId, string textContent, string mediaContent)
        {
            Id = id;
            EventId = eventId;
            StudentId = studentId;
            TextContent = textContent;
            MediaContent = mediaContent;
        }
    }    
}