using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendAcceptanceException : DomainException
    {
        public override string Code { get; } = "friend_acceptance_failure";
        public Guid RequesterId { get; }
        public Guid AcceptorId { get; }

        public FriendAcceptanceException(Guid requesterId, Guid acceptorId)
            : base($"Failure in accepting friend request from {requesterId} to {acceptorId}.")
        {
            RequesterId = requesterId;
            AcceptorId = acceptorId;
        }
    }
}
