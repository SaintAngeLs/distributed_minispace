using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Application.Events.External.Posts
{
    [Message("posts")]
    public class PostCreated : IEvent
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

        public PostCreated(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
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
