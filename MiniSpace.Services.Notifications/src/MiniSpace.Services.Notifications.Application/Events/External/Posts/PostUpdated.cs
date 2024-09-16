using Convey.CQRS.Events;
using Convey.MessageBrokers;
using System;

namespace MiniSpace.Services.Notifications.Application.Events.External.Posts
{
    [Message("posts")]
    public class PostUpdated : IEvent
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string TextContent { get; }
        public string Context { get; }
        public bool ShouldNotify { get; }

        public PostUpdated(Guid postId, Guid? userId, Guid? organizationId, 
            Guid? eventId, string textContent, string context, bool shouldNotify)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            Context = context;
            ShouldNotify = shouldNotify;
        }
    }
}
