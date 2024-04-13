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

        public void SetToBePublished(DateTime publishDate)
        {
            State = State.ToBePublished;
            PublishDate = publishDate;
        }
        public void SetPublished() => State = State.Published;
        public void SetInDraft() => State = State.InDraft;
        public void SetHidden() => State = State.Hidden;
        public void SetReported() => State = State.Reported;
        
        public static Post Create(AggregateId id, Guid eventId, Guid studentId, string textContent,
            string mediaContent, State state, DateTime? publishDate)
        {
            CheckContent(id, textContent, mediaContent);
            
            return new Post(id, eventId, studentId, textContent, mediaContent, state, publishDate);
        }

        public void Update(string textContent, string mediaContent)
        {
            CheckContent(Id, textContent, mediaContent);

            TextContent = textContent;
            MediaContent = mediaContent;
        }

        private static void CheckContent(AggregateId id, string textContent, string mediaContent)
        {
            if (string.IsNullOrWhiteSpace(textContent) && string.IsNullOrWhiteSpace(mediaContent))
            {
                throw new InvalidPostContentException(id);
            }
        }
    }    
}
