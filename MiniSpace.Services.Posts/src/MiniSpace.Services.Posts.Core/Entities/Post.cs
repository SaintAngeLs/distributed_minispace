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
        public PostType Type { get; private set; }
        public string Title { get; private set; }

        // New fields to track the page ownership
        public Guid? PageOwnerId { get; private set; } // Owner of the page where the post is published (could be User or Organization)
        public PageOwnerType PageOwnerType { get; private set; } // Specifies if the page belongs to a user or an organization

        // New fields to track reposts
        public Guid? OriginalPostId { get; private set; }
        public bool IsRepost => OriginalPostId.HasValue;

        // Constructor
        public Post(Guid id, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, PostContext context, DateTime? publishDate,
            PostType type, string title = null, VisibilityStatus visibility = VisibilityStatus.Visible,
            DateTime? updatedAt = null, Guid? pageOwnerId = null, PageOwnerType pageOwnerType = PageOwnerType.User, 
            Guid? originalPostId = null)
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
            Type = type;
            Title = title;
            Visibility = visibility;
            PageOwnerId = pageOwnerId;
            PageOwnerType = pageOwnerType;
            OriginalPostId = originalPostId;

            AddEvent(new PostCreatedEvent(Id));
        }

        // Factory methods for different types of posts

        public static Post CreateBlogPost(Guid id, Guid userId, string title, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate,
            VisibilityStatus visibility = VisibilityStatus.Visible, Guid? pageOwnerId = null, PageOwnerType pageOwnerType = PageOwnerType.User)
        {
            CheckTextContent(id, textContent, PostType.BlogPost);

            return new Post(id, userId, null, null, textContent, mediaFiles, createdAt, state, PostContext.UserPage,
                publishDate ?? createdAt, PostType.BlogPost, title, visibility, updatedAt: null, pageOwnerId: pageOwnerId, pageOwnerType: pageOwnerType);
        }

        public static Post CreateSocialPost(Guid id, Guid userId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate,
            VisibilityStatus visibility = VisibilityStatus.Visible, Guid? pageOwnerId = null, PageOwnerType pageOwnerType = PageOwnerType.User)
        {
            CheckTextContent(id, textContent, PostType.SocialPost);

            return new Post(id, userId, null, null, textContent, mediaFiles, createdAt, state, PostContext.UserPage,
                publishDate ?? createdAt, PostType.SocialPost, title: null, visibility, updatedAt: null, pageOwnerId: pageOwnerId, pageOwnerType: pageOwnerType);
        }

        public static Post CreateOrganizationPost(Guid id, Guid userId, Guid organizationId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate,
            VisibilityStatus visibility = VisibilityStatus.Visible)
        {
            CheckTextContent(id, textContent, PostType.SocialPost);

            return new Post(id, userId, organizationId, null, textContent, mediaFiles, createdAt, state, PostContext.OrganizationPage,
                publishDate ?? createdAt, PostType.SocialPost, title: null, visibility, updatedAt: null, pageOwnerId: organizationId, pageOwnerType: PageOwnerType.Organization);
        }

        public static Post CreateEventPost(Guid id, Guid userId, Guid organizationId, Guid eventId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate,
            VisibilityStatus visibility = VisibilityStatus.Visible)
        {
            CheckTextContent(id, textContent, PostType.SocialPost);

            return new Post(id, userId, organizationId, eventId, textContent, mediaFiles, createdAt, state, PostContext.EventPage,
                publishDate ?? createdAt, PostType.SocialPost, title: null, visibility, updatedAt: null, pageOwnerId: organizationId, pageOwnerType: PageOwnerType.Organization);
        }

        // Factory method for repost
        public static Post CreateRepost(Guid id, Guid userId, Post originalPost, DateTime createdAt, State state)
        {
            if (originalPost == null)
            {
                throw new InvalidPostStateException("Original post cannot be null.");
            }

            return new Post(id, userId, null, originalPost.EventId, originalPost.TextContent, originalPost.MediaFiles, 
                createdAt, state, originalPost.Context, publishDate: createdAt, originalPost.Type, 
                originalPost.Title, originalPost.Visibility, pageOwnerId: userId, pageOwnerType: PageOwnerType.User, 
                originalPostId: originalPost.Id);
        }

        // Methods to manage post state

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

        // Update the state based on the current time
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

        public void Update(string textContent, IEnumerable<string> mediaFiles, DateTime now)
        {
            CheckTextContent(Id, textContent, Type);

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

        // Repost the post
        public void Repost(Guid userId, DateTime now)
        {
            if (IsRepost)
            {
                throw new InvalidPostStateException("Cannot repost a repost.");
            }

            AddEvent(new PostRepostedEvent(Id, userId, OriginalPostId ?? Id, now));
        }

        // Validation Methods
        private static void CheckTextContent(AggregateId id, string textContent, PostType postType)
        {
            int maxTextLength = postType == PostType.BlogPost ? 40000 : 5000;

            if (string.IsNullOrWhiteSpace(textContent) || textContent.Length > maxTextLength)
            {
                throw new InvalidPostTextContentException(id, maxTextLength);
            }
        }

        private static void CheckPublishDate(AggregateId id, State state, DateTime publishDate, DateTime now)
        {
            if (publishDate <= now)
            {
                throw new InvalidPostPublishDateException(id, state, publishDate, now);
            }
        }

        public static Post CreateForUser(Guid id, Guid userId, string textContent, IEnumerable<string> mediaFiles,
            DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility)
        {
            return new Post(id, userId, null, null, textContent, mediaFiles, createdAt, state, PostContext.UserPage,
                publishDate ?? createdAt, PostType.SocialPost, visibility: visibility, pageOwnerId: userId, pageOwnerType: PageOwnerType.User);
        }

        public static Post CreateForOrganization(Guid id, Guid organizationId, Guid? userId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility)
        {
            return new Post(id, userId, organizationId, null, textContent, mediaFiles, createdAt, state, PostContext.OrganizationPage,
                publishDate ?? createdAt, PostType.SocialPost, visibility: visibility, pageOwnerId: organizationId, pageOwnerType: PageOwnerType.Organization);
        }

        public static Post CreateForEvent(Guid id, Guid eventId, Guid? userId, Guid? organizationId, string textContent,
            IEnumerable<string> mediaFiles, DateTime createdAt, State state, DateTime? publishDate, VisibilityStatus visibility)
        {
            return new Post(id, userId, organizationId, eventId, textContent, mediaFiles, createdAt, state, PostContext.EventPage,
                publishDate ?? createdAt, PostType.SocialPost, visibility: visibility, pageOwnerId: organizationId ?? userId, pageOwnerType: organizationId.HasValue ? PageOwnerType.Organization : PageOwnerType.User);
        }
    }
}

