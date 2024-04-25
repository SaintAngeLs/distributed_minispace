namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class AlreadyFriendsException : AppException
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public override string Code { get; } = "already_friends";
        public AlreadyFriendsException(Guid requesterId, Guid friendId)
            : base($"Already friends: {requesterId} and {friendId}")
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}

