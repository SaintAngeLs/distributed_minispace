using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Core.Exceptions
{
    public class InvalidPostPublishDateException : DomainException
    {
        public override string Code { get; } = "invalid_post_publish_date";
        public Guid Id { get; }
        public State State { get; }
        public DateTime PublishDate { get; }
        public DateTime Now { get; }

        public InvalidPostPublishDateException(Guid id, State state, DateTime publishDate, DateTime now) : base(
            $"Post with id: {id} has invalid publish date. Today date: " +
            $"'{now}' must be older than publish date: '{publishDate}'.")
        {
            Id = id;
            State = state;
            PublishDate = publishDate;
            Now = now;
        }
    }
}