namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class FriendRequestNotFoundException : AppException
    {
        public override string Code { get; } = "friend_request_not_found";
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendRequestNotFoundException(Guid inviterId, Guid inviteeId)
            : base($"Friend request from inviter '{inviterId}' to invitee '{inviteeId}' was not found.")
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
