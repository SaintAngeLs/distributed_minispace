using MiniSpace.Services.Posts.Core.Events;
using MiniSpace.Services.Posts.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Post : AggregateRoot
    {
        public Guid? UserId { get; private set; }
        public Guid? OrganizationId { get; private set; }
        public Guid? EventId { get; private set; }
        public string TextContent { get; private set; }
        public IEnumerable<string> MediaFiles { get; private set; } 
        public State State { get; private set; }
        public DateTime? PublishDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public PostContext Context { get; private set; }
        public VisibilityStatus Visibility { get; private set; } 

        public Post(Guid id, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, PostContext context, DateTime? publishDate,
            VisibilityStatus visibility = VisibilityStatus.Visible, DateTime? updatedAt = null)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            MediaFiles = mediaFiles ?? new List<string>();
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            State = state;
            PublishDate = publishDate;
            Context = context;
            Visibility = visibility;

            AddEvent(new PostCreatedEvent(Id));
        }

        public void SetToBePublished(DateTime publishDate, DateTime now)
        {
            CheckPublishDate(Id, State.ToBePublished, publishDate, now);
            State = State.ToBePublished;
            PublishDate = publishDate;
            UpdatedAt = now;

            AddEvent(new PostPublishedEvent(Id));
        }

        public void SetPublished(DateTime now)
        {
            State = State.Published;
            PublishDate = now;
            UpdatedAt = now;

            AddEvent(new PostPublishedEvent(Id));
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

        public void SetVisibility(VisibilityStatus visibility, DateTime now)
        {
            Visibility = visibility;
            UpdatedAt = now;

            AddEvent(new PostVisibilityChangedEvent(Id, visibility, now));
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

        public void ChangeState(State newState, DateTime? publishDate, DateTime now)
        {
            if (newState == State.ToBePublished && publishDate.HasValue)
            {
                SetToBePublished(publishDate.Value, now);
            }
            else if (newState == State.Published)
            {
                SetPublished(now);
            }
            else if (newState == State.InDraft)
            {
                SetInDraft(now);
            }
            else if (newState == State.Reported)
            {
                SetReported(now);
            }
            else
            {
                throw new InvalidPostStateException(newState.ToString());
            }
        }

        public static Post CreateForUser(Guid id, Guid userId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility = VisibilityStatus.Visible)
        {
            CheckTextContent(id, textContent);

            return new Post(id, userId, null, null, textContent, mediaFiles, createdAt, state, PostContext.UserPage,
                publishDate ?? createdAt, visibility);
        }

        public static Post CreateForOrganization(Guid id, Guid organizationId, Guid? userId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility = VisibilityStatus.Visible)
        {
            CheckTextContent(id, textContent);

            return new Post(id, userId, organizationId, null, textContent, mediaFiles, createdAt, state, PostContext.OrganizationPage,
                publishDate ?? createdAt, visibility);
        }

        public static Post CreateForEvent(Guid id, Guid eventId, Guid? userId, Guid? organizationId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility = VisibilityStatus.Visible)
        {
            CheckTextContent(id, textContent);

            return new Post(id, userId, organizationId, eventId, textContent, mediaFiles, createdAt, state, PostContext.EventPage,
                publishDate ?? createdAt, visibility);
        }


        public void Update(string textContent, IEnumerable<string> mediaFiles, DateTime now)
        {
            CheckTextContent(Id, textContent);

            TextContent = textContent;
            MediaFiles = mediaFiles;
            UpdatedAt = now;

            AddEvent(new PostUpdatedEvent(Id));
        }

        public void RemoveMediaFile(string mediaFileUrl, DateTime now)
        {
            if (!MediaFiles.Contains(mediaFileUrl))
            {
                throw new MediaFileNotFoundException(mediaFileUrl, Id);
            }

            MediaFiles = MediaFiles.Where(mf => mf != mediaFileUrl).ToList();
            UpdatedAt = now;

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
