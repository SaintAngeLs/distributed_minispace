using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class PendingFriendDeclined : IDomainEvent
    {
        public Student Requester { get; }
        public Student Decliner { get; }

        public PendingFriendDeclined(Student requester, Student decliner)
        {
            Requester = requester;
            Decliner = decliner;
        }
    }

}
