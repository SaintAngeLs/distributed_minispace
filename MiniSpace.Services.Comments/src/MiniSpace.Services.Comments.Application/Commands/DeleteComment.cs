using Convey.CQRS.Commands;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class DeleteComment : ICommand
    {
        public Guid CommentId { get; }

        public DeleteComment(Guid CommentId) => CommentId = CommentId;
    }    
}
