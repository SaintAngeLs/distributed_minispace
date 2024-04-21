using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipStateChanged : IDomainEvent
    {
        public Friend Student { get; }
        public State PreviousState { get; }

        public FriendshipStateChanged(Friend student, State previousState)
        {
            Student = student;
            PreviousState = previousState;
        }
    }    
}
