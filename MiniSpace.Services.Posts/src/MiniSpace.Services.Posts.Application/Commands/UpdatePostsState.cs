using Convey.CQRS.Commands;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class UpdatePostsState : ICommand
    {
        public DateTime Now { get; set; }

        public UpdatePostsState(DateTime now)
        {
            Now = now;
        }
    }    
}
