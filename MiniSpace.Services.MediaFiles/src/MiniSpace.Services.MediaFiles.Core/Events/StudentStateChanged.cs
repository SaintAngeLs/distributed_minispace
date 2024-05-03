using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Events
{
    public class StudentStateChanged : IDomainEvent
    {
        public Student Student { get; }
        public State PreviousState { get; }

        public StudentStateChanged(Student student, State previousState)
        {
            Student = student;
            PreviousState = previousState;
        }
    }    
}
