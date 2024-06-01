using MiniSpace.Services.Posts.Core.Exceptions;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Post : AggregateRoot
    {
        public Guid EventId { get; private set; }
        public Guid OrganizerId { get; private set; }
        public string TextContent { get; private set; }
        public IEnumerable<Guid> MediaFiles { get; private set; }
        public State State { get; private set; }
        public DateTime? PublishDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        public Post(Guid id, Guid eventId, Guid organizerId, string textContent,
            IEnumerable<Guid> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, 
            DateTime? updatedAt = null)
        {
            Id = id;
            EventId = eventId;
            OrganizerId = organizerId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
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
            PublishDate = now;
            UpdatedAt = now;
        }
        
        public void SetInDraft(DateTime now)
        {
            State = State.InDraft;
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
            IEnumerable<Guid> mediaFiles, DateTime createdAt, State state, DateTime? publishDate)
        {
            CheckTextContent(id, textContent);

            return new Post(id, eventId, studentId, textContent, mediaFiles, createdAt, state, 
                publishDate ?? createdAt);
        }

        public void Update(string textContent, IEnumerable<Guid> mediaFiles, DateTime now)
        {
            CheckTextContent(Id, textContent);

            TextContent = textContent;
            MediaFiles = mediaFiles;
            UpdatedAt = now;
        }
        
        public void RemoveMediaFile(Guid mediaFileId, DateTime now)
        {
            var mediaFile = MediaFiles.SingleOrDefault(mf => mf == mediaFileId);
            if (mediaFile == Guid.Empty)
            {
                throw new MediaFileNotFoundException(mediaFileId, Id);
            }
        }
        
        private static void CheckTextContent(AggregateId id, string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent) || textContent.Length > 5000)
            {
                throw new InvalidPostTextContentException(id);
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
