using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class SentFriendRequestWithdraw : ICommand
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public SentFriendRequestWithdraw(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
