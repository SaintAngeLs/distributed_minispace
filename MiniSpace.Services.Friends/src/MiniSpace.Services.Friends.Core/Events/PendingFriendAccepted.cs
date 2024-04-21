using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class PendingFriendAccepted : IDomainEvent
    {
        public Student Requester { get; }
        public Student Acceptor { get; }

        public PendingFriendAccepted(Student requester, Student acceptor)
        {
            Requester = requester;
            Acceptor = acceptor;
        }
    }
}
