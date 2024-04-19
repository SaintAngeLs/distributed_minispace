using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class PublishDateNullException : DomainException
    {
        public override string Code { get; } = "publish_date_null";
        public Guid Id { get; }
        public State State { get; }
        
        public PublishDateNullException(Guid id, State state) : base(
            $"Publish date cannot be null for post: {id}.")
        {
            Id = id;
            State = state;
        }
    }
}