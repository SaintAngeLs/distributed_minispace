namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public class GalleryImageDocument
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OrganizationId { get; set; }
    }
}