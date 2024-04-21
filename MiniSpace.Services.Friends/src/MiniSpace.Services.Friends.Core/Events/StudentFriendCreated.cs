using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class StudentFriendCreated : IDomainEvent
    {
        public Friend Student { get; }

        public StudentFriendCreated(Friend student)
        {
            Student = student;
        }
    }
}
