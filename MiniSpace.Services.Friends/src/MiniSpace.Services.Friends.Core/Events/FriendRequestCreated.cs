using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestCreated : IDomainEvent
    {
        public Student Requester { get; }
        public Student Requestee { get; }

        public FriendRequestCreated(Student requester, Student requestee)
        {
            Requester = requester;
            Requestee = requestee;
        }
    }
}
