namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostPublishDateException : DomainException
    {
        public override string Code { get; } = "invalid_post_publish_date";
        public Guid Id;
        public DateTime PublishDate { get; }
        public DateTime Now { get; }

        public InvalidPostPublishDateException(Guid id, DateTime publishDate, DateTime now) : base(
            $"Post with id: {id} has invalid publish date. Today date: " +
            $"'{now}' must be older than publish date: '{publishDate}'.")
        {
            Id = id;
            PublishDate = publishDate;
            Now = now;
        }
    }
}