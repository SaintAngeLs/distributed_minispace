using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class StudentUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentUpdated(Student student)
        {
            Student = student;
        }
    }    
}
