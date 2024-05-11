using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendAdded : IDomainEvent
    {
        public Student Requester { get; private set; }
        public Student Friend { get; private set; }

        public FriendAdded(Student requester, Student friend)
        {
            Requester = requester;
            Friend = friend;
        }
    }
}
