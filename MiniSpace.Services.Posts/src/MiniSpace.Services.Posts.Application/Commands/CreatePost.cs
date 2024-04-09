using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class CreatePost : ICommand
    {
        public Guid PostId { get; }
        public Guid EventId { get; }
        public Guid StudentId { get; }
        public string TextContent { get; }
        public string MediaContent { get; }

        public CreatePost(Guid postId, Guid eventId, Guid studentId, string textContent, string mediaContent)
        {
            PostId = postId == Guid.Empty ? Guid.NewGuid() : postId;
            EventId = eventId;
            StudentId = studentId;
            TextContent = textContent;
            MediaContent = mediaContent;
        }
    }    
}
