using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Events
{
    public class UserBlockedEvent : IDomainEvent
    {
        public Student Student { get; }
        public Guid BlockedUserId { get; }

        public UserBlockedEvent(Student student, Guid blockedUserId)
        {
            Student = student;
            BlockedUserId = blockedUserId;
        }
    }
}
