using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class CreatePost : ICommand
    {
        public Guid PostId { get; }
        public Guid EventId { get; }
        public Guid OrganizerId { get; }
        public string TextContent { get; }
        public IEnumerable<Guid> MediaFiles { get; }
        public string State { get; }
        public DateTime? PublishDate { get; }

        public CreatePost(Guid postId, Guid eventId, Guid organizerId, string textContent,
            IEnumerable<Guid> mediaFiles, string state, DateTime? publishDate)
        {
            PostId = postId == Guid.Empty ? Guid.NewGuid() : postId;
            EventId = eventId;
            OrganizerId = organizerId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
            State = state;
            PublishDate = publishDate;
        }
    }    
}
