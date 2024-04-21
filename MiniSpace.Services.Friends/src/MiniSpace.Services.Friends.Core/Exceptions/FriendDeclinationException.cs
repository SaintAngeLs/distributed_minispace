namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendDeclinationException : DomainException
    {
        public override string Code { get; } = "friend_declination_failure";
        public Guid RequesterId { get; }
        public Guid DeclinerId { get; }

        public FriendDeclinationException(Guid requesterId, Guid declinerId)
            : base($"Declination of friend request from {requesterId} by {declinerId} failed.")
        {
            RequesterId = requesterId;
            DeclinerId = declinerId;
        }
    }
}
