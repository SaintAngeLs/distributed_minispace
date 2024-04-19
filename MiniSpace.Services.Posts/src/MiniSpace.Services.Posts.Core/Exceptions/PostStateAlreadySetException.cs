using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class PostStateAlreadySetException : DomainException
    {
        public override string Code { get; } = "post_state_already_set";
        public Guid Id { get; }
        public State State { get; }

        public PostStateAlreadySetException(Guid id, State state) : base(
            $"Post: {id} has state already set to: {state}.")
        {
            Id = id;
            State = state;
        }
    }    
}
