namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class InvalidFriendshipStateException : DomainException
    {
        public override string Code { get; } = "invalid_friendship_state";
        public Guid FriendshipId { get; }
        public string CurrentState { get; }
        public string RequiredState { get; }

        public InvalidFriendshipStateException(Guid friendshipId, string currentState, string requiredState)
            : base($"Cannot confirm friendship: Current state '{currentState}' does not meet required state '{requiredState}'.")
        {
            FriendshipId = friendshipId;
            CurrentState = currentState;
            RequiredState = requiredState;
        }
    }
}
