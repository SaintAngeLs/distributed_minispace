using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendshipStateChanged : IDomainEvent
    {
        public Student Student { get; }
        public State PreviousState { get; }

        public FriendshipStateChanged(Student student, State previousState)
        {
            Student = student;
            PreviousState = previousState;
        }
    }    
}
