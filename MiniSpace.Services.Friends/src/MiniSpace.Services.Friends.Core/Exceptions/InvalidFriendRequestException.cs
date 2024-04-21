using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class InvalidFriendRequestException : DomainException
    {
        public override string Code { get; } = "invalid_friend_request";
        public Guid RequesterId { get; }
        public Guid RequesteeId { get; }

        public InvalidFriendRequestException(Guid requesterId, Guid requesteeId)
            : base($"Invalid friend request from {requesterId} to {requesteeId}.")
        {
            RequesterId = requesterId;
            RequesteeId = requesteeId;
        }
    }
}
