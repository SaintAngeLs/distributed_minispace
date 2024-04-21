using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class StudentFriendCreated : IDomainEvent
    {
        public Student Student { get; }

        public StudentFriendCreated(Student student)
        {
            Student = student;
        }
    }
}
