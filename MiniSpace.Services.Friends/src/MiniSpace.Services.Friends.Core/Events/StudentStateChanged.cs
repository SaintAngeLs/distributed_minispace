using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendShipStateChanged : IDomainEvent
    {
        public Student Student { get; }
        public State PreviousState { get; }

        public FriendShipStateChanged(Student student, State previousState)
        {
            Student = student;
            PreviousState = previousState;
        }
    }    
}
