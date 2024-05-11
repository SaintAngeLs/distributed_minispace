namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class InvalidFriendRequestException : AppException
    {
        public override string Code { get; } = "invalid_friend_request";
        public InvalidFriendRequestException() : base("Invalid friend request.")
        {
        }
    }
}

