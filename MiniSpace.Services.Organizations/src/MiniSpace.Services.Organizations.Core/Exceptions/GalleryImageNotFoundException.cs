namespace MiniSpace.Services.Organizations.Core.Exceptions
{
    public class GalleryImageNotFoundException : DomainException
    {
        public override string Code { get; } = "gallery_image_not_found";
        public Guid ImageId { get; }

        public GalleryImageNotFoundException(Guid imageId)
            : base($"Gallery image with ID: '{imageId}' was not found.")
        {
            ImageId = imageId;
        }
    }
}
