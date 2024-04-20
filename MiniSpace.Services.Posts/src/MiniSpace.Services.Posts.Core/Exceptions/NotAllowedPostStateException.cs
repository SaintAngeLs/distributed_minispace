using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class NotAllowedPostStateException : DomainException
    {
        public override string Code { get; } = "not_allowed_post_state";
        public Guid Id { get; }
        public State State { get; }

        public NotAllowedPostStateException(Guid id, State state) : base(
            $"State: {state} is not allowed for post: {id}.")
        {
            Id = id;
            State = state;
        }
    }
}
