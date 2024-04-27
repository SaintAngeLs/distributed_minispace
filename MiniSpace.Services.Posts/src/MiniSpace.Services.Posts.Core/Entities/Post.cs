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
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        public Post(Guid id, Guid eventId, Guid studentId, string textContent,
            string mediaContent, DateTime createdAt, State state, DateTime? publishDate, DateTime? updatedAt = null)
        {
            Id = id;
            EventId = eventId;
            StudentId = studentId;
            TextContent = textContent;
            MediaContent = mediaContent;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            State = state;
            PublishDate = publishDate;
        }

        public void SetToBePublished(DateTime publishDate, DateTime now)
        {
            CheckPublishDate(Id, State.ToBePublished, publishDate, now);
            State = State.ToBePublished;
            PublishDate = publishDate;
            UpdatedAt = now;
        }
        
        public void SetPublished(DateTime now)
        {
            State = State.Published;
            PublishDate = null;
            UpdatedAt = now;
        }
        
        public void SetInDraft(DateTime now)
        {
            State = State.InDraft;
            PublishDate = null;
            UpdatedAt = now;
        }

        public void SetHidden(DateTime now)
        {
            State = State.Hidden;
            PublishDate = null;
            UpdatedAt = now;
        }

        public void SetReported(DateTime now)
        {
            State = State.Reported;
            PublishDate = null;
            UpdatedAt = now;
        }

        public bool UpdateState(DateTime now)
        {
            if (State == State.ToBePublished && PublishDate <= now)
            {
                SetPublished(now);
                return true;
            }
            
            return false;
        }
        
        public static Post Create(AggregateId id, Guid eventId, Guid studentId, string textContent,
            string mediaContent, DateTime createdAt, State state, DateTime? publishDate)
        {
            CheckContent(id, textContent, mediaContent);
            
            return new Post(id, eventId, studentId, textContent, mediaContent, createdAt, state, publishDate);
        }

        public void Update(string textContent, string mediaContent, DateTime now)
        {
            CheckContent(Id, textContent, mediaContent);

            TextContent = textContent;
            MediaContent = mediaContent;
            UpdatedAt = now;
        }
        
        private static void CheckContent(AggregateId id, string textContent, string mediaContent)
        {
            if (string.IsNullOrWhiteSpace(textContent) && string.IsNullOrWhiteSpace(mediaContent))
            {
                throw new InvalidPostContentException(id);
            }
        }

        private static void CheckPublishDate(AggregateId id, State state, DateTime publishDate, DateTime now)
        {
            if (publishDate <= now)
            {
                throw new InvalidPostPublishDateException(id, state, publishDate, now);
            }
        }
    }    
}
