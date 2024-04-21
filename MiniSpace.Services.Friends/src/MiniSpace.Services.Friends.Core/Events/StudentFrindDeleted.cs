using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class StudentFrindDeleted : IDomainEvent
    {
        public Student Student { get; }

        public StudentFrindDeleted(Student student)
        {
            Student = student;
        }
    }
}
