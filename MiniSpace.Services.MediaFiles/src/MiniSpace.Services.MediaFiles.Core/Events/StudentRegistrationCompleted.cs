using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Events
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
