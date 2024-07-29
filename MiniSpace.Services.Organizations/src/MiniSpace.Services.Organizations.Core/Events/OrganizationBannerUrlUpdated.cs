namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationBannerUrlUpdated : IDomainEvent
    {
        public Guid OrganizationId { get; }
        public string BannerUrl { get; }
        public DateTime UpdatedAt { get; }

        public OrganizationBannerUrlUpdated(Guid organizationId, string bannerUrl, DateTime updatedAt)
        {
            OrganizationId = organizationId;
            BannerUrl = bannerUrl;
            UpdatedAt = updatedAt;
        }
        
    }
}
