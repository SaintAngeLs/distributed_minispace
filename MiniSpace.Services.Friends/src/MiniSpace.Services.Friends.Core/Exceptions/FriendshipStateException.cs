using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendshipStateException : DomainException
    {
        public override string Code { get; } = "friendship_state_error";
        public Guid StudentId { get; }
        public FriendState AttemptedState { get; }
        public FriendState CurrentState { get; }

        public FriendshipStateException(Guid studentId, FriendState attemptedState, FriendState currentState)
            : base($"Attempt to change friendship state to {attemptedState} from {currentState} failed for student ID {studentId}.")
        {
            StudentId = studentId;
            AttemptedState = attemptedState;
            CurrentState = currentState;
        }
    }
}
