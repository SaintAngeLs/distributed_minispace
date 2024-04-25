namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class FriendshipNotFoundException : AppException
    {
        public override string Code { get; } = "friendship_not_found";
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public FriendshipNotFoundException(Guid requesterId, Guid friendId) : base($"Friendship between requester ID {requesterId} and friend ID {friendId} was not found.")
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }

        public FriendshipNotFoundException(Guid friendId) : base($"Friendship involving friend ID {friendId} was not found.")
        {
            FriendId = friendId;
        }

    }
}

