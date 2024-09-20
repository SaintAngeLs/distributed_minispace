using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendshipStateException : DomainException
    {
        public override string Code { get; } = "friendship_state_error";
        public Guid UserId { get; }
        public FriendState AttemptedState { get; }
        public FriendState CurrentState { get; }

        public FriendshipStateException(Guid userId, FriendState attemptedState, FriendState currentState)
            : base($"Attempt to change friendship state to {attemptedState} from {currentState} failed for user ID {userId}.")
        {
            UserId = userId;
            AttemptedState = attemptedState;
            CurrentState = currentState;
        }
    }
}
