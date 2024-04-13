using MiniSpace.Services.Posts.Core.Exceptions;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Post : AggregateRoot
    {
        public Guid EventId { get; private set; }
        public Guid StudentId { get; private set; }
        public string TextContent { get; private set; }
        public string MediaContent { get; private set; }
        public State State { get; private set; }
        public DateTime? PublishDate { get; private set; }

        public Post(Guid id, Guid eventId, Guid studentId, string textContent,
            string mediaContent, State state, DateTime? publishDate)
        {
            Id = id;
            EventId = eventId;
            StudentId = studentId;
            TextContent = textContent;
            MediaContent = mediaContent;
            State = state;
            PublishDate = publishDate;
        }

        public static Post Create(AggregateId id, Guid eventId, Guid studentId, string textContent,
            string mediaContent, State state, DateTime? publishDate)
        {
            CheckTextContent(id, textContent);
            CheckMediaContent(id, mediaContent);
            
            return new Post(id, eventId, studentId, textContent, mediaContent, state, publishDate);
        }

        public void Update(string textContent, string mediaContent)
        {
            CheckTextContent(Id, textContent);
            CheckMediaContent(Id, mediaContent);

            TextContent = textContent;
            MediaContent = mediaContent;
        }
        
        private static void CheckTextContent(AggregateId id, string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent))
            {
                throw new InvalidPostTextContentException(id, textContent);
            }
        }
        
        private static void CheckMediaContent(AggregateId id, string mediaContent)
        {
            if (string.IsNullOrWhiteSpace(mediaContent))
            {
                throw new InvalidPostMediaContentException(id, mediaContent);
            }
        }
    }    
}
