using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentRegistrationCompleted : IDomainEvent
    {
        public Student Student { get; }

        public StudentRegistrationCompleted(Student student)
        {
            Student = student;
        }
    }    
}
