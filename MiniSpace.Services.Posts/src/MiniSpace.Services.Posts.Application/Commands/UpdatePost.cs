using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class UpdatePost : ICommand
    {
        public Guid PostId { get; }
        public string TextContent { get; }
        public string MediaContent { get; }

        public UpdatePost(Guid postId, string textContent, string mediaContent)
        {
            PostId = postId;
            TextContent = textContent;
            MediaContent = mediaContent;
        }
    }    
}
