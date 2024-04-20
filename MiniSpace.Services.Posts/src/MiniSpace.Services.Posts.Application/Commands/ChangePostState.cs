using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class ChangePostState : ICommand
    {
        public Guid PostId { get; }
        public string State { get; }
        public DateTime? PublishDate { get; }

        public ChangePostState(Guid postId, string state, DateTime? publishDate)
        {
            PostId = postId;
            State = state;
            PublishDate = publishDate;
        }
    }    
}
