
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Core.Events
{
    public class FriendRequestSent : ICommand
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendRequestSent(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
