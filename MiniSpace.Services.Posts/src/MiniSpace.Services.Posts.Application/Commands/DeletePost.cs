using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class DeletePost : ICommand
    {
        public Guid PostId { get; }

        public DeletePost(Guid postId) => PostId = postId;
    }    
}
