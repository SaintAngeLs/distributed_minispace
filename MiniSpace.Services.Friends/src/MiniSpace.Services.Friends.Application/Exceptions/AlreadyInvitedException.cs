namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class AlreadyInvitedException : AppException
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }
        public override string Code { get; } = "already_invited";

        public AlreadyInvitedException(Guid inviterId, Guid inviteeId)
            : base($"A pending friend request already exists between: {inviterId} and {inviteeId}")
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
