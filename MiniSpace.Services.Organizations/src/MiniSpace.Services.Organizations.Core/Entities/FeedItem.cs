namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class FeedItem
    {
        public Guid Id { get; }
        public Guid OrganizationId { get; }
        public Guid CreatorId { get; }
        public string Content { get; }
        public DateTime CreatedAt { get; }

        public FeedItem(Guid organizationId, Guid creatorId, string content)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            CreatorId = creatorId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
