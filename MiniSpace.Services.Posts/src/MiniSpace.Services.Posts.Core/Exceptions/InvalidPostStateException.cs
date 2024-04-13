using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostStateException : DomainException
    {
        public override string Code { get; } = "invalid_post_state";
        public string InvalidState { get; }

        public InvalidPostStateException(string invalidState) : base(
            $"String: {invalidState} cannot be parsed to valid post state.")
        {
            InvalidState = invalidState;
        }
    }    
}
