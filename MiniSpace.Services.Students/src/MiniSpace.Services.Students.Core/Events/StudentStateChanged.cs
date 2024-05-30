using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
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
