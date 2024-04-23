namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class AlreadyFriendsException : AppException
    {
        public override string Code { get; } = "already_friends";
        public AlreadyFriendsException() : base("Already friends or invitation already sent.")
        {
        }
    }
}

