using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserUnblockedEvent : IDomainEvent
    {
        public Student Student { get; }
        public Guid UnblockedUserId { get; }

        public UserUnblockedEvent(Student student, Guid unblockedUserId)
        {
            Student = student;
            UnblockedUserId = unblockedUserId;
        }
    }
}
