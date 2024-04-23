
namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class UnauthorizedFriendActionException : AppException
    {
        public override string Code { get; } = "unauthorized_friend_action";
        public UnauthorizedFriendActionException() : base("You are not authorized to perform this action.")
        {
        }
    }
}

