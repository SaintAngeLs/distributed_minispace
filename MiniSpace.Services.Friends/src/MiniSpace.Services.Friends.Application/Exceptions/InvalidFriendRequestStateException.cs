namespace MiniSpace.Services.Friends.Application.Exceptions
{
    public class InvalidFriendRequestStateException : AppException
    {
        public override string Code { get; } = "invalid_friend_request_state";
        public Guid RequesterId { get; }
        public Guid FriendId { get; }
        public string CurrentState { get; }

        public InvalidFriendRequestStateException(Guid requesterId, Guid friendId, string currentState) 
            : base($"Friend request between requester ID {requesterId} and friend ID {friendId} is in an invalid state '{currentState}' for this operation.")
        {
            RequesterId = requesterId;
            FriendId = friendId;
            CurrentState = currentState;
        }
    }
}
