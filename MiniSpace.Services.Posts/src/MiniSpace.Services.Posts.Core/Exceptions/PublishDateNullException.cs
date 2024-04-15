namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class PublishDateNullException : DomainException
    {
        public override string Code { get; } = "publish_date_null";
        public Guid Id { get; }
        
        public PublishDateNullException(Guid id) : base(
            $"Publish date cannot be null for post: {id}.")
        {
            Id = id;
        }
    }
}