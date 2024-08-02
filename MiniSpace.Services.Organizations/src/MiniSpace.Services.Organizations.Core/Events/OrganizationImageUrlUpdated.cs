namespace MiniSpace.Services.Organizations.Core.Events
{
      public class OrganizationImageUrlUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public string ImageUrl { get; }
        public DateTime UpdatedAt { get; }

        public OrganizationImageUrlUpdated(Guid organizationId, string imageUrl, DateTime updatedAt)
        {
            OrganizationId = organizationId;
            ImageUrl = imageUrl;
            UpdatedAt = updatedAt;
        }
    }
}
