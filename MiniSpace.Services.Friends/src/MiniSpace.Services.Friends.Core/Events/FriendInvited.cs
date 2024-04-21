using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendInvited : IDomainEvent
    {
        public Student Inviter { get; }
        public Student Invitee { get; }

        public FriendInvited(Student inviter, Student invitee)
        {
            Inviter = inviter;
            Invitee = invitee;
        }
    }
}
