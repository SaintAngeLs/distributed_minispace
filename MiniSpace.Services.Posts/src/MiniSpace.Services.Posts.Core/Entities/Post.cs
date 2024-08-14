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

        private Post(Guid id, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, PostContext context, DateTime? publishDate,
            DateTime? updatedAt = null)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            State = state;
            PublishDate = publishDate;
            Context = context;

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

            // Raise the PostPublishedEvent when the post is published
            AddEvent(new PostPublishedEvent(Id));
        }

        public void SetInDraft(DateTime now)
        {
            State = State.InDraft;
            PublishDate = null;
            UpdatedAt = now;

            // Raise an event if necessary (e.g., PostDraftedEvent)
        }

        public void SetReported(DateTime now)
        {
            State = State.Reported;
            PublishDate = null;
            UpdatedAt = now;

            // Raise an event if necessary (e.g., PostReportedEvent)
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

        public static Post CreateForUser(Guid id, Guid userId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate)
        {
            CheckTextContent(id, textContent);

            return new Post(id, userId, null, null, textContent, mediaFiles, createdAt, state, PostContext.UserPage,
                publishDate ?? createdAt);
        }

        public static Post CreateForOrganization(Guid id, Guid organizationId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate)
        {
            CheckTextContent(id, textContent);

            return new Post(id, null, organizationId, null, textContent, mediaFiles, createdAt, state, PostContext.OrganizationPage,
                publishDate ?? createdAt);
        }

        public static Post CreateForEvent(Guid id, Guid eventId, Guid? userId, Guid? organizationId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate)
        {
            CheckTextContent(id, textContent);

            return new Post(id, userId, organizationId, eventId, textContent, mediaFiles, createdAt, state, PostContext.EventPage,
                publishDate ?? createdAt);
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

            // Raise an event if necessary (e.g., PostMediaFileRemovedEvent)
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
