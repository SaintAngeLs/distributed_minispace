using MiniSpace.Services.MediaFiles.Core.Entities;

namespace MiniSpace.Services.MediaFiles.Core.Events
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
