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
        public string State { get; }
        public DateTime? PublishDate { get; }

        public CreatePost(Guid postId, Guid eventId, Guid studentId, string textContent,
            string mediaContent, string state, DateTime? publishDate)
        {
            PostId = postId == Guid.Empty ? Guid.NewGuid() : postId;
            EventId = eventId;
            StudentId = studentId;
            TextContent = textContent;
            MediaContent = mediaContent;
            State = state;
            PublishDate = publishDate;
        }
    }    
}
