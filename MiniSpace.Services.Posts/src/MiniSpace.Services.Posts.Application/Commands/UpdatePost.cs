using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class UpdatePost : ICommand
    {
        public Guid PostId { get; }
        public string TextContent { get; }
        public IEnumerable<Guid> MediaFiles { get; }

        public UpdatePost(Guid postId, string textContent, IEnumerable<Guid> mediaFiles)
        {
            PostId = postId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
        }
    }    
}
