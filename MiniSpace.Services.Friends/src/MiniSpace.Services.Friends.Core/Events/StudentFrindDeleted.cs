using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class StudentFrindDeleted : IDomainEvent
    {
        public Friend Student { get; }

        public StudentFrindDeleted(Friend student)
        {
            Student = student;
        }
    }
}
