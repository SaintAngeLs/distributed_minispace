using MiniSpace.Services.Posts.Core.Exceptions;

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

        public Post Create(AggregateId id, Guid eventId, Guid studentId,
            string textContent, string mediaContent)
        {
            CheckTextContent(textContent);
            CheckMediaContent(mediaContent);
            
            return new Post(id, eventId, studentId, textContent, mediaContent);
        }

        public void Update(string textContent, string mediaContent)
        {
            CheckTextContent(textContent);
            CheckMediaContent(mediaContent);

            TextContent = textContent;
            MediaContent = mediaContent;
        }
        
        private void CheckTextContent(string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent))
            {
                throw new InvalidPostTextContentException(Id, textContent);
            }
        }
        
        private void CheckMediaContent(string mediaContent)
        {
            if (string.IsNullOrWhiteSpace(mediaContent))
            {
                throw new InvalidPostMediaContentException(Id, mediaContent);
            }
        }
    }    
}
