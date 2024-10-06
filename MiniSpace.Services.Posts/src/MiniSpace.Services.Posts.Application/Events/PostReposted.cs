using System;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostReposted : IEvent
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string TextContent { get; }
        public IEnumerable<string> MediaFiles { get; }
        public string Context { get; }
        public string Visibility { get; }
        public bool ShouldNotify { get; }

        public PostReposted(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId,
            string textContent, IEnumerable<string> mediaFiles, string context, string visibility, bool shouldNotify)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
            Context = context;
            Visibility = visibility;
            ShouldNotify = shouldNotify;
        }
    }
}
