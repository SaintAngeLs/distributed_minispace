using Convey.CQRS.Events;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostUpdated : IEvent
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string TextContent { get; }
        public IEnumerable<string> MediaFilesUrls { get; }
        public string Context { get; }
        public string Visibility { get; }
        public bool ShouldNotify { get; }

        public PostUpdated(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
            IEnumerable<string> mediaFilesUrls, string context, string visibility, bool shouldNotify)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            MediaFilesUrls = mediaFilesUrls;
            Context = context;
            Visibility = visibility;
            ShouldNotify = shouldNotify; 
        }
    }
}
