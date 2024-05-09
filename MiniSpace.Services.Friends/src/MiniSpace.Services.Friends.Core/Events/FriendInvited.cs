using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendInvited : IDomainEvent
    {
        public Friend Inviter { get; }
        public Friend Invitee { get; }

        public FriendInvited(Friend inviter, Friend invitee)
        {
            Inviter = inviter;
            Invitee = invitee;
        }
    }
}
