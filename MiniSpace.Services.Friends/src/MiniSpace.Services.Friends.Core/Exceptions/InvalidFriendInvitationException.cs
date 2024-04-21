using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class InvalidFriendInvitationException : DomainException
    {
        public override string Code { get; } = "invalid_friend_invitation";
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public InvalidFriendInvitationException(Guid inviterId, Guid inviteeId)
            : base($"Invalid invitation from {inviterId} to {inviteeId}.")
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
