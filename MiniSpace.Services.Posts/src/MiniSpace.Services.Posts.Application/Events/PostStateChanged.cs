using Convey.CQRS.Events;

namespace MiniSpace.Services.Posts.Application.Events
{
    public class PostStateChanged : IEvent
    {
        public Guid PostId { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public PostStateChanged(Guid postId, string currentState, string previousState)
        {
            PostId = postId;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }    
}
