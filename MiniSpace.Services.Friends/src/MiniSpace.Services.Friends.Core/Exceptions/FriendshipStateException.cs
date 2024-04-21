namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendshipStateException : DomainException
    {
        public override string Code { get; } = "friendship_state_error";
        public Guid StudentId { get; }
        public State NewState { get; }

        public FriendshipStateException(Guid studentId, State newState)
            : base($"Cannot change state to {newState} for student {studentId}.")
        {
            StudentId = studentId;
            NewState = newState;
        }
    }
}
