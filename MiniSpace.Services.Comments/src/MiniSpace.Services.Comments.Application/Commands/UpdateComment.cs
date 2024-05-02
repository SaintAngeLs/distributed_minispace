using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class UpdateComment : ICommand
    {
        public Guid CommentId { get; }
        public string TextContent { get; }

        public UpdateComment(Guid commentId, string textContent)
        {
            CommentId = commentId;
            TextContent = textContent;
        }
    }    
}
