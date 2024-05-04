
namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class UnauthorizedFriendActionException : AppException
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public override string Code { get; } = "unauthorized_friend_action";
        public UnauthorizedFriendActionException(Guid requesterId, Guid friendId) : base("You are not authorized to perform this action.")
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}

